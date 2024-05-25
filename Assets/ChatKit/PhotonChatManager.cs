
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using TMPro;
using System.Linq;


#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using Photon.Chat.Demo;
#endif

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{

    public string[] ChannelsToJoinOnConnect; // set in inspector. Demo channels to join automatically.


    public int HistoryLengthToFetch; // set in inspector. Up to a certain degree, previously sent messages can be fetched for context

    public string UserName { get; set; }

    [SerializeField] private ChattingItem _chattingItem;
    [SerializeField] private RectTransform _chatScrollParent;
    private List<ChattingItem> _chattingItemList = new();

    private string selectedChannelName; // mainly used for GUI/input

    public ChatClient chatClient;

#if !PHOTON_UNITY_NETWORKING
    public ChatAppSettings ChatAppSettings => this.chatAppSettings;

    [SerializeField]
#endif
    protected internal ChatAppSettings chatAppSettings;


    public RectTransform ChatPanel;     // set in inspector (to enable/disable panel)
    public TMP_InputField InputFieldChat;   // set in inspector
    public TMP_Text CurrentChannelText;     // set in inspector


    private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();


    // private static string WelcomeText = "Welcome to chat. Type \\help to list commands.";
    private static string HelpText = "\n    -- HELP --\n" +
        "To subscribe to channel(s) (channelnames are case sensitive) :  \n" +
            "\t<color=#E07B00>\\subscribe</color> <color=green><list of channelnames></color>\n" +
            "\tor\n" +
            "\t<color=#E07B00>\\s</color> <color=green><list of channelnames></color>\n" +
            "\n" +
            "To leave channel(s):\n" +
            "\t<color=#E07B00>\\unsubscribe</color> <color=green><list of channelnames></color>\n" +
            "\tor\n" +
            "\t<color=#E07B00>\\u</color> <color=green><list of channelnames></color>\n" +
            "\n" +
            "To switch the active channel\n" +
            "\t<color=#E07B00>\\join</color> <color=green><channelname></color>\n" +
            "\tor\n" +
            "\t<color=#E07B00>\\j</color> <color=green><channelname></color>\n" +
            "\n" +
            "To send a private message: (username are case sensitive)\n" +
            "\t\\<color=#E07B00>msg</color> <color=green><username></color> <color=green><message></color>\n" +
            "\n" +
            "To change status:\n" +
            "\t\\<color=#E07B00>state</color> <color=green><stateIndex></color> <color=green><message></color>\n" +
            "<color=green>0</color> = Offline " +
            "<color=green>1</color> = Invisible " +
            "<color=green>2</color> = Online " +
            "<color=green>3</color> = Away \n" +
            "<color=green>4</color> = Do not disturb " +
            "<color=green>5</color> = Looking For Group " +
            "<color=green>6</color> = Playing" +
            "\n\n" +
            "To clear the current chat tab (private chats get closed):\n" +
            "\t<color=#E07B00>\\clear</color>";


    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //this.ChatPanel.gameObject.SetActive(false);

        this.UserName = "user" + Environment.TickCount % 99; //made-up username\

#if PHOTON_UNITY_NETWORKING
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
#endif

        bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);

        if (!appIdPresent)
            Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");

        Connect();
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

        //CurrentChannelText.text = "Connecting...";
    }

    /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnDestroy.</summary>
    public void OnDestroy()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnApplicationQuit.</summary>
    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void Update()
    {
        if (this.chatClient != null)
            this.chatClient.Service();

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


    public int TestLength = 2048;
    private byte[] testBytes = new byte[2048];

    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }
        if ("test".Equals(inputLine))
        {
            if (this.TestLength != this.testBytes.Length)
            {
                this.testBytes = new byte[this.TestLength];
            }

            this.chatClient.SendPrivateMessage(this.chatClient.AuthValues.UserId, this.testBytes, true);
        }


        bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(this.selectedChannelName);
        string privateChatTarget = string.Empty;
        if (doingPrivateChat)
        {
            // the channel name for a private conversation is (on the client!!) always composed of both user's IDs: "this:remote"
            // so the remote ID is simple to figure out

            string[] splitNames = this.selectedChannelName.Split(new char[] { ':' });
            privateChatTarget = splitNames[1];
        }
        //UnityEngine.Debug.Log("selectedChannelName: " + selectedChannelName + " doingPrivateChat: " + doingPrivateChat + " privateChatTarget: " + privateChatTarget);


        if (inputLine[0].Equals('\\'))
        {
            string[] tokens = inputLine.Split(new char[] { ' ' }, 2);
            if (tokens[0].Equals("\\help"))
            {
                this.PostHelpToCurrentChannel();
            }
            if (tokens[0].Equals("\\state"))
            {
                int newState = 0;


                List<string> messages = new List<string>();
                messages.Add("i am state " + newState);
                string[] subtokens = tokens[1].Split(new char[] { ' ', ',' });

                if (subtokens.Length > 0)
                    newState = int.Parse(subtokens[0]);

                if (subtokens.Length > 1)
                    messages.Add(subtokens[1]);

                this.chatClient.SetOnlineStatus(newState, messages.ToArray()); // this is how you set your own state and (any) message
            }
            else if ((tokens[0].Equals("\\subscribe") || tokens[0].Equals("\\s")) && !string.IsNullOrEmpty(tokens[1]))
            {
                this.chatClient.Subscribe(tokens[1].Split(new char[] { ' ', ',' }));
            }
            else if ((tokens[0].Equals("\\unsubscribe") || tokens[0].Equals("\\u")) && !string.IsNullOrEmpty(tokens[1]))
            {
                this.chatClient.Unsubscribe(tokens[1].Split(new char[] { ' ', ',' }));
            }
            else if (tokens[0].Equals("\\clear"))
            {
                if (doingPrivateChat)
                {
                    this.chatClient.PrivateChannels.Remove(this.selectedChannelName);
                }
                else
                {
                    ChatChannel channel;
                    if (this.chatClient.TryGetChannel(this.selectedChannelName, false, out channel))
                        channel.ClearMessages();
                }
            }
            else if (tokens[0].Equals("\\msg") && !string.IsNullOrEmpty(tokens[1]))
            {
                string[] subtokens = tokens[1].Split(new char[] { ' ', ',' }, 2);
                if (subtokens.Length < 2) return;

                string targetUser = subtokens[0];
                string message = subtokens[1];
                this.chatClient.SendPrivateMessage(targetUser, message);
            }
            else if ((tokens[0].Equals("\\join") || tokens[0].Equals("\\j")) && !string.IsNullOrEmpty(tokens[1]))
            {
                string[] subtokens = tokens[1].Split(new char[] { ' ', ',' }, 2);

                // If we are already subscribed to the channel we directly switch to it, otherwise we subscribe to it first and then switch to it implicitly
                if (this.channelToggles.ContainsKey(subtokens[0]))
                    this.ShowChannel(subtokens[0]);
                else
                    this.chatClient.Subscribe(new string[] { subtokens[0] });
            }
#if CHAT_EXTENDED
                else if ((tokens[0].Equals("\\nickname") || tokens[0].Equals("\\nick") ||tokens[0].Equals("\\n")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    if (!doingPrivateChat)
                    {
                        this.chatClient.SetCustomUserProperties(this.selectedChannelName, this.chatClient.UserId, new Dictionary<string, object> {{"Nickname", tokens[1]}});
                    }

                }
#endif
            else
            {
                Debug.Log("The command '" + tokens[0] + "' is invalid.");
            }
        }
        else
        {
            if (doingPrivateChat)
            {
                this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
            }
            else
            {
                this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
            }
        }
    }

    public void PostHelpToCurrentChannel()
    {
        //this.CurrentChannelText.text += HelpText;
    }

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnConnected()
    {
        //CurrentChannelText.text = string.Empty;

        if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
            this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);

        //this.ChatPanel.gameObject.SetActive(true);

        this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
    }

    public void OnDisconnected()
    {
        Debug.Log("OnDisconnected()");
    }

    public void OnChatStateChange(ChatState state)
    {
        // use OnConnected() and OnDisconnected()
        // this method might become more useful in the future, when more complex states are being used.


    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        // in this demo, we simply send a message into each channel. This is NOT a must have!
        foreach (string channel in channels)
            this.chatClient.PublishMessage(channel, "says 'hi'."); // you don't HAVE to send a msg on join but you could.

        Debug.Log("OnSubscribed: " + string.Join(", ", channels));

        // Switch to the first newly created channel
        this.ShowChannel(channels[0]);
    }

    /// <inheritdoc />
    public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
    {
        Debug.LogFormat("OnSubscribed: {0}, users.Count: {1} Channel-props: {2}.", channel, users.Length, properties.ToStringFull());
    }

    private void InstantiateChannelButton(string channelName)
    {
        if (this.channelToggles.ContainsKey(channelName))
        {
            Debug.Log("Skipping creation for an existing channel toggle.");
            return;
        }
    }


    public void OnUnsubscribed(string[] channels)
    {
        foreach (string channelName in channels)
        {
            if (this.channelToggles.ContainsKey(channelName))
            {
                Toggle t = this.channelToggles[channelName];
                Destroy(t.gameObject);

                this.channelToggles.Remove(channelName);

                Debug.Log("Unsubscribed from channel '" + channelName + "'.");

                // Showing another channel if the active channel is the one we unsubscribed from before
                if (channelName == this.selectedChannelName && this.channelToggles.Count > 0)
                {
                    IEnumerator<KeyValuePair<string, Toggle>> firstEntry = this.channelToggles.GetEnumerator();
                    firstEntry.MoveNext();

                    this.ShowChannel(firstEntry.Current.Key);

                    firstEntry.Current.Value.isOn = true;
                }
            }
            else
            {
                Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
            }
        }
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

        if (channel != null)
        {
            channel.Add("Bot", msg, 0); //TODO: how to use msgID?
        }
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
        //this.CurrentChannelText.text = channel.ToStringMessages();
        Debug.Log($"Final");

        _chattingItemList.ForEach((item) => Destroy(item.gameObject));
        _chattingItemList.Clear();

        for (int i = 0; i < channel.Messages.Count; i++)
        {
            string msg = $"{channel.Senders[i]} : {channel.Messages[i]}";
            var chat = Instantiate(_chattingItem, _chatScrollParent);
            chat.SetText(msg);
            _chattingItemList.Add(chat);
        }
        foreach (var item in channel.Messages)
        {
        }

        Debug.Log("ShowChannel: " + this.selectedChannelName);

        foreach (KeyValuePair<string, Toggle> pair in this.channelToggles)
        {
            pair.Value.isOn = pair.Key == channelName ? true : false;
        }
    }

    public void OpenDashboard()
    {
        Application.OpenURL("https://dashboard.photonengine.com");
    }




}
