using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public abstract class Weapons : MonoBehaviourPun
{
    public int Damage;
    public LayerMask mask;
    public float Cadence;
    protected float _timeSinceShoot;
    public float reloadSpeed;
    public int ammo;
    public int maxAmmo;
    public Transform origin;
    public bool usePhoton;
    public bool canShoot=true;
    public Action shootEvent;

    public abstract void Shoot();
    public abstract void Shoot(int userID);
    //public abstract void Reload();
    public virtual void Start()
    {
        #if PHOTON_UNITY_NETWORKING
        usePhoton = true;
        #endif
    }

    [PunRPC]
    public void UpdateBullet(int myAmmo)
    {
        ammo = myAmmo;
    }

}
