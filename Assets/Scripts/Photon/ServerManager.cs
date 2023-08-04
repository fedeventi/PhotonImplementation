using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using DPR;


public class ServerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public TMP_Text clients;
    public Transform[] spawnPoints;
    public Player _server;
    public GameObject serverPanel;
    public TextMeshProUGUI endingText;
    public static ServerManager Instance;
    bool _endNotificationSend;
    public GameObject deadPanel;
    public Image hurtImage;

    bool endGameWin;
    bool winner;

    public bool EndGame
    {
        get
        {
            return endGameWin;
        }
    }

    public bool Wining
    {
        get
        {
            return winner;
        }
        set
        {
            winner = value;
        }
    }

    public bool AllPlayersInRoom
    {
        get
        {
            return PhotonNetwork.CurrentRoom.PlayerCount - 1 >= 4;
        }
    }

    public int playersCount
    {
        get
        {
            return PhotonNetwork.CurrentRoom.PlayerCount - 1;
        }
    }

    private void Update()
    {
        if(photonView.IsMine)
            if(!_endNotificationSend)
                CheckSurvivors();

        if(winner && PhotonNetwork.CurrentRoom.PlayerCount == 1) // Solo queda el sv
        {
            Debug.LogError("CLOSEROOM  SV GO MENU");
            GoToMenu();
        }
    }



    void Start()
    {
        if (Instance == null)
        {
            serverPanel.SetActive(true);
            Instance = this;
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("CreateServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
            }
        }

       
    }

    public void CheckSurvivors()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        var survivor = SpawnManager.Instance.All_soldiers.Where(soldier => !soldier.dead);
        if (survivor.Count() == 1)
        {
            photonView.RPC("NotifyLastSurvivor", survivor.FirstOrDefault().photonView.Controller);
            _endNotificationSend = true;
        }

    }

    [PunRPC]
    public void NotifyLastSurvivor()
    {
        endingText.text = "you re the last player, you won,  press \"space\" to end or continue playing to increase your points";
        endGameWin = true;
    }


    [PunRPC]
    void CreateServer(Player client)
    {
        _server = client;

        if (PhotonNetwork.LocalPlayer != _server)
        {
            Destroy(serverPanel);
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber-1].position, spawnPoints[0].rotation);
        }

        clients.text = $"Players in room: {PhotonNetwork.CurrentRoom.PlayerCount - 1}";
       
    }


    public void GoToMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Loby");
    }


}
