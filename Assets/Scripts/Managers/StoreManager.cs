using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using DPR;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class StoreManager : MonoBehaviourPun
{

    public TextMeshProUGUI coins, shotgun, uzi, readyText;
    int _coins, _shotgun, _uzi;
    public bool ready;
    public int readyPlayers;
    public Text waiting;


    public Text debugg;


    void Start()
    {
        _coins = UserData.coins;
        _shotgun = UserData.shotgunBullets;
        _uzi = UserData.uziBullets;
        coins.text = $"Coins: {_coins}";
        if (PhotonNetwork.IsMasterClient)
        {
            ready = true;
        }
        Debug.Log(PhotonNetwork.IsMasterClient);
    }

    void Update()
    {
        shotgun.text = $"Shotgun Ammo: {_shotgun}";
        uzi.text = $"Uzi Ammo: {_uzi}";
        coins.text = $"Coins: {_coins}";
    }

    public void BuyShotgunAmmo()
    {
        if (_coins < 100 ) return;
        _shotgun += 20;
        //StartCoroutine(DiscountCoins(100));
        _coins -= 100;
        StartCoroutine(IncreaseShotgunBullets(100));

    }

    public void BuyUziAmmo()
    {
        if (_coins < 50) return;
        _uzi += 100;
        //StartCoroutine(DiscountCoins(50));
        _coins -= 50;
        StartCoroutine(IncreaseUziBullets(50));

    }

    IEnumerator DiscountCoins(int cost)
    {
        int previousCoins = _coins;
        _coins -= cost;
        UserData.coins = _coins;
        while (previousCoins > _coins)
        {
            previousCoins--;
            coins.text = $"Coins: {previousCoins}";
            yield return new WaitForSeconds(.003f);
        }

    }

    public void LoadLevel()
    {
        
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel("TestLevel");
    }



    IEnumerator IncreaseUziBullets(int coins)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", UserData.userID);
        form.AddField("price", coins);
        form.AddField("cantBullets", 100);
        form.AddField("weapponID", 1);

        UnityWebRequest www = UnityWebRequest.Post(WebServer.SERVER_NAME + "/updateBullets.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log($"Server error, please retry later.\n\n" +
                $"{www.error}");


            yield return null;
        }
        else
        {
            UpdateData(www);
        }

    }


    public void UpdateData(UnityWebRequest www)
    {
        if (Helper.GetData(www.downloadHandler.text, "MESSAGE:") == "buy successfully!")
        {
            Debug.Log("-->" + www.downloadHandler.text);
            UserData.coins = int.Parse(Helper.GetData(www.downloadHandler.text, "COINS:"));
            UserData.uziBullets = int.Parse(Helper.GetData(www.downloadHandler.text, "USZIBULLETS:"));
            UserData.shotgunBullets = int.Parse(Helper.GetData(www.downloadHandler.text, "SHOTGUNBULLETS:"));
        }
    }

    IEnumerator IncreaseShotgunBullets(int coins)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", UserData.userID);
        form.AddField("price", coins);
        form.AddField("cantBullets", 20);
        form.AddField("weapponID", 2);

        UnityWebRequest www = UnityWebRequest.Post(WebServer.SERVER_NAME + "/updateBullets.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log($"Server error, please retry later.\n\n" +
                $"{www.error}");


            yield return null;
        }
        else
        {
            UpdateData(www);
        }
    }

    public void Ready()
    {
        if (PhotonNetwork.IsMasterClient) return;
        ready = !ready;
        
        readyText.text = ready ? "not Ready" : "Ready";
        photonView.RPC("TellServerReady", RpcTarget.AllBuffered, ready);
        
    }

    [PunRPC]
    public void TellServerReady(bool  ready)
    {
        
        readyPlayers += ready ? 1 : -1;
        waiting.text = $"waiting for players ({readyPlayers}/4)";
        if (PhotonNetwork.IsMasterClient)
        {
            if (readyPlayers >= 4)
            {
                readyPlayers = 0;
                LoadLevel();
            }
        }
    }

}
