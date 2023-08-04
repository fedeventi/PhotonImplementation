using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using DPR;

public class NetManager : MonoBehaviourPunCallbacks
{
    public static NetManager Instance { get; set; }

    public string username;
    public TMP_Text debugInfo;
    public TMP_InputField nickNameField;
    public TMP_InputField nameRoom;
    public Button create, connect;
    bool isConnected = false;

    void Start()
    {
        ConnectToServer();

        if (username != null)
            username = UserData.userName;
        
        if (nickNameField != null)
            nickNameField.text = username;
    }

    private void Update()
    {
        create.interactable = isConnected;
        connect.interactable = isConnected;
        if (PhotonNetwork.IsMasterClient)
            nickNameField.text = "Server";
    }

    void ConnectToServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
                if (debugInfo != null) debugInfo.text += $"\n Connect server";
                else
                if (debugInfo != null) debugInfo.text += $"\n Failing Connecting to Server";
        }
    }

    void ChangeNickName()
    {
        if (nickNameField == null) return;

        if (nickNameField.text != username)
        {
            username = nickNameField.text;
            PhotonNetwork.LocalPlayer.NickName = username;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = username;
        }
        if (PhotonNetwork.LocalPlayer==PhotonNetwork.MasterClient) {
            Debug.Log("masterclient");
            PhotonNetwork.LocalPlayer.NickName = "server"; }

    }

    public void CreateToRoom()
    {
        ChangeNickName();
        var option = new RoomOptions();
        option.MaxPlayers = 5;
        option.IsOpen = true;
        option.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(nameRoom.text, option, TypedLobby.Default);
    }

    public void ConnectToRoom()
    {
        ChangeNickName();
        var option = new RoomOptions();
        option.MaxPlayers = 5;
        option.IsOpen = true;
        option.IsVisible = true;
        PhotonNetwork.JoinRandomRoom();
    }


    public void CreateToRandomRoom()
    {
        ChangeNickName();
        var roomName = Random.Range(1000, 9999).ToString();
        var option = new RoomOptions();
        option.MaxPlayers = 12;
        option.IsOpen = true;
        option.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(roomName, option, TypedLobby.Default);
    }


    public override void OnConnectedToMaster()
    {
        isConnected = true;
        PhotonNetwork.AutomaticallySyncScene = true;
        if (debugInfo != null) debugInfo.text += $"\n You are connected to the { PhotonNetwork.CloudRegion.ToUpper() } server";
    }

    public override void OnCreatedRoom()
    {
        if (debugInfo != null) debugInfo.text += $"\n Room Created";
        Debug.Log("Room Created");
    }

    public override void OnJoinedRoom()
    {
        if (debugInfo != null) debugInfo.text += $"\n Joined Room";
        PhotonNetwork.LoadLevel("store");
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (debugInfo != null) debugInfo.text += $"\n Fail Joining Room \n {message}";
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (debugInfo != null) debugInfo.text += $"\n { returnCode} Fail Creating Room {message}";
    }
}
