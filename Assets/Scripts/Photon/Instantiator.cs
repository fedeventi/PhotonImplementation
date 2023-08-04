using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Instantiator : MonoBehaviourPun
{
    public Transform[] spawnPoints;
    public GameObject pj;
    public SpawnManager spawnManager;

    void Start()
    {
        var num = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PhotonNetwork.NickName))
        {
           pj = PhotonNetwork.Instantiate(pj.name, spawnPoints[num].position, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
           spawnManager.tgt = pj.GetComponent<SoldierModel>();
        }
        else
        {
            pj = PhotonNetwork.Instantiate(pj.name, spawnPoints[num].position, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
            spawnManager.tgt = pj.GetComponent<SoldierModel>();
        }

        //var cam = FindObjectOfType<CameraController>();
        //cam.character = pj;
        //cam.SetValues();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (var item in spawnPoints)
        {
            Gizmos.DrawWireSphere(item.position, .5f);
        }
       
    }

}
