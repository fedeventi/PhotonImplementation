using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using System;
public class Chat : MonoBehaviourPun
{
    public InputField textField;
    public Text displayedText;
    public ScrollRect scroll;
    string _color;

    void Start()
    {               
        _color = "#"+ColorUtility.ToHtmlStringRGB(UnityEngine.Random.ColorHSV());
        if(!PhotonNetwork.IsMasterClient)
            JoinedAdvise();        
    }

    void Update()
    {
        if (textField != null)
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (textField.text != "")
                    SendMessageToServer();

            }
        
    }

    public void JoinedAdvise()
    {
        photonView.RPC("SendMessageToOthers", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName, "has joined", _color,true);
    }

    public void SendMessageToServer()
    {
        
        if (textField.text == "") return;
        photonView.RPC("SendMessageToOthers", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName, textField.text, _color,false);
        textField.text = "";
        
        EventSystem.current.SetSelectedGameObject(textField.gameObject);
    }
    public string CheckPuteadas(string text)
    {
        var newText = text.ToLower();
        foreach (var item in Enum.GetValues(typeof(Puteadas)))
        {
            if (newText.Contains(item.ToString()))
            {
                Debug.Log("no digas palabrotas");
                newText = newText.Replace(item.ToString(), "####");
            }


        }
        return newText;
    }
    [PunRPC]
    public void SendMessageToOthers(string username, string message, string colorHex,bool hasJoined)
    {
        var text = CheckPuteadas(message);
        photonView.RPC("SendMessage", RpcTarget.AllBuffered, username, text, colorHex,hasJoined);
    }
    [PunRPC]
    public void SendMessage(string username, string message, string colorHex, bool hasJoined)
    {
        if(!hasJoined)
           displayedText.text+=  "\n"+$" <color={colorHex}>{ username }</color> "+": " + message;
        else
           displayedText.text+=  "\n"+$" <color={colorHex}>{ username }</color> "+ message;
        StartCoroutine(Down());
        
    }
    
   
    IEnumerator Down()
    {
        yield return new WaitForSeconds(0.5f);
        scroll.normalizedPosition = new Vector2(0, 0);
    }
    //no ver las puteadas



    public enum Puteadas
    {
        pelotudo,
        pelotuda,
        puto,
        puta,
        sorete,
        pija,
        concha,
        conchudo,
        conchuda,
        mierda,
        tonto,
        estupido,
        idiota,
        boludo,
        boluda
    }



}
