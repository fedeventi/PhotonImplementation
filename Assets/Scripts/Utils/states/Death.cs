using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using DPR;

public class Death<T> : State<T>
{
    ZombieModel _zombie;
    Action<int,int> onDeath;
    public Death(ZombieModel zombie)
    {
        _zombie = zombie;
        onDeath += GameManager.instance.AddPoints;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        _zombie.photonView.RPC("Death", RpcTarget.All);

        //Debug.LogError($"ID:   {UserData.userID} | _zombie.killID: {_zombie.killID} <---------");
        //if (_zombie.killID == UserData.userID)
            onDeath.Invoke(10, _zombie.killID);

    }
    

    
    public override void OnUpdate()
    {

    }
    public override void OnSleep()
    {
        base.OnSleep();
    }
}
