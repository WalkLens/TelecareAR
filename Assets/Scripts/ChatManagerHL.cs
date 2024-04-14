using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ChatManagerHL : MonoBehaviourPunCallbacks
{
    public GameObject m_Content;
    public GameObject chatBox;
    PhotonView photonview;
    string m_strUserName;
    public void Start()
    {
        Screen.SetResolution(960, 600, false);
        PhotonNetwork.ConnectUsingSettings();

        photonview = GetComponent<PhotonView>();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        m_strUserName = PhotonNetwork.LocalPlayer.NickName;
        photonview.RPC("RPC_AddChatMessage", RpcTarget.All, m_strUserName + "님 접속");
    }
    // public void SetInputReturn()
    // {
    //     string strMessage = m_strUserName + " : " + inputField.text;
    //     photonview.RPC("RPC_AddChatMessage", RpcTarget.All, strMessage);
    // }

    void AddChatMessage(string message)
    {
        GameObject goText = Instantiate(chatBox, m_Content.transform);

        GameObject chatBox_Frontplate = goText.transform.GetChild(2).gameObject;
        GameObject chatBox_AnimatedContent = chatBox_Frontplate.transform.GetChild(0).gameObject;
        GameObject chatBox_Text = chatBox_AnimatedContent.transform.GetChild(1).gameObject;

        chatBox_Text.GetComponent<TextMeshProUGUI>().text = message;
    }

    [PunRPC]
    void RPC_AddChatMessage(string message)
    {
        Debug.Log("RPC_Chat : " + message);
        AddChatMessage(message);
    }
}