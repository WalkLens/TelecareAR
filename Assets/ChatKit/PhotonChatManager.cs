
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using TMPro;

#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using Photon.Chat.Demo;
#endif

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    public ChatClient chatClient;
    
#if !PHOTON_UNITY_NETWORKING
    public ChatAppSettings ChatAppSettings => this.chatAppSettings;
#endif

    [SerializeField] private TMP_InputField InputFieldChat;   // set in inspector
    [SerializeField] private TMP_Text CurrentChannelText;     // set in inspector

    public string[] ChannelsToJoinOnConnect; // set in inspector. Demo channels to join automatically.

    public int HistoryLengthToFetch; // set in inspector. Up to a certain degree, previously sent messages can be fetched for context

    public string UserName { get; set; }

    protected internal ChatAppSettings chatAppSettings;

    private string selectedChannelName; // mainly used for GUI/input

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        this.UserName = "user" + Environment.TickCount % 99; //made-up username\

#if PHOTON_UNITY_NETWORKING
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
#endif

        bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);

        if (!appIdPresent)
            Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");

        Connect();
    }

    public void Update()
    {
        if (this.chatClient != null)
            this.chatClient.Service();
    }

    public void OnDestroy()
    {
        if (this.chatClient != null)
            this.chatClient.Disconnect();
    }

    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
            this.chatClient.Disconnect();
    }

    public void Connect()
    {
        this.chatClient = new ChatClient(this);
#if !UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = true;
#endif
        this.chatClient.AuthValues = new AuthenticationValues(this.UserName);
        this.chatClient.ConnectUsingSettings(this.chatAppSettings);

        //this.ChannelToggleToInstantiate.gameObject.SetActive(false);
        Debug.Log("Connecting as: " + this.UserName);

        CurrentChannelText.text = "Connecting...";
    }

    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendChatMessage(this.InputFieldChat.text);
            this.InputFieldChat.text = "";
        }
    }

    public void OnClickSend()
    {
        if (this.InputFieldChat != null)
        {
            this.SendChatMessage(this.InputFieldChat.text);
            this.InputFieldChat.text = "";
        }
    }

    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
            return;

        this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
    }

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
            Debug.LogError(message);
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
            Debug.LogWarning(message);
        else
            Debug.Log(message);
    }

    public void OnConnected()
    {
        CurrentChannelText.text = string.Empty;

        if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
            this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);

        this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
    }

    public void OnDisconnected()
    {
        Debug.Log("OnDisconnected()");
    }

    public void OnChatStateChange(ChatState state)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        foreach (string channel in channels)
            this.chatClient.PublishMessage(channel, "says 'hi'."); // you don't HAVE to send a msg on join but you could.

        Debug.Log("OnSubscribed: " + string.Join(", ", channels));

        this.ShowChannel(channels[0]);
    }

    public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
    {
        Debug.LogFormat("OnSubscribed: {0}, users.Count: {1} Channel-props: {2}.", channel, users.Length, properties.ToStringFull());
    }

    public void OnUnsubscribed(string[] channels)
    {

    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(this.selectedChannelName))
            this.ShowChannel(this.selectedChannelName);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    public void OnChannelPropertiesChanged(string channel, string userId, Dictionary<object, object> properties)
    {
        Debug.LogFormat("OnChannelPropertiesChanged: {0} by {1}. Props: {2}.", channel, userId, Extensions.ToStringFull(properties));
    }

    public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
    {
        Debug.LogFormat("OnUserPropertiesChanged: (channel:{0} user:{1}) by {2}. Props: {3}.", channel, targetUserId, senderUserId, Extensions.ToStringFull(properties));
    }

    /// <inheritdoc />
    public void OnErrorInfo(string channel, string error, object data)
    {
        Debug.LogFormat("OnErrorInfo for channel {0}. Error: {1} Data: {2}", channel, error, data);
    }

    public void AddMessageToSelectedChannel(string msg)
    {
        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(this.selectedChannelName, out channel);
        if (!found)
        {
            Debug.Log("AddMessageToSelectedChannel failed to find channel: " + this.selectedChannelName);
            return;
        }

        channel?.Add("Bot", msg, 0); //TODO: how to use msgID?
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        this.selectedChannelName = channelName;
        this.CurrentChannelText.text = channel.ToStringMessages();
        Debug.Log("ShowChannel: " + this.selectedChannelName);
    }

    public void OpenDashboard()
    {
        Application.OpenURL("https://dashboard.photonengine.com");
    }
}
