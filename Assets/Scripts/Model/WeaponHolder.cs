using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponHolder : MonoBehaviourPun
{
    // Start is called before the first frame update
    SoldierModel soldierModel;
    public int ShotgunBullets
    {
        get => transform.GetChild(1).GetComponent<Weapons>().ammo;
        //set=> transform.GetChild(1).GetComponent<Weapons>().ammo=value;
    }
    public int UziBullets
    {
        get => transform.GetChild(2).GetComponent<Weapons>().ammo;
        //set => transform.GetChild(2).GetComponent<Weapons>().ammo = value;
    }
    int max;
    int index;
    void Start()
    {
        max = transform.childCount;

        photonView.RPC("GetSoldierModel",RpcTarget.All);
        photonView.RPC("AssignShootActions", RpcTarget.All);
        


    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (soldierModel.dead)
        {
            photonView.RPC("DeactivateWeaponsBehaviours", RpcTarget.All);

        }
        DisplayWeaponAmmo();
        if (transform.GetChild(index).GetComponent<Weapons>().ammo <= 0)
        photonView.RPC("DecreaseIndex", RpcTarget.All);
            
        if (Input.GetAxis("Mouse ScrollWheel")<0) 
        {
            photonView.RPC("DecreaseIndex", RpcTarget.All);
        }
        else if (Input.GetAxis("Mouse ScrollWheel")>0)
        {
            photonView.RPC("IncreaseIndex", RpcTarget.All);
          
        }
        photonView.RPC("ActivateWeapons", RpcTarget.All);
    }
    [PunRPC]
    void ActivateWeapons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {

            transform.GetChild(i).gameObject.SetActive(index == i ? true : false);
        }
    }
    [PunRPC]
    void GetSoldierModel()
    {
        Transform current = transform;
        int exceptionCount = 20;
        while (soldierModel == null && exceptionCount > 0)
        {
            soldierModel = current.parent.GetComponent<SoldierModel>();
            current = current.parent;
            exceptionCount--;
        }
    }
    [PunRPC]
    void AssignShootActions()
    {
        
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<Weapons>() && soldierModel.SoldierView)
                transform.GetChild(i).GetComponent<Weapons>().shootEvent += soldierModel.SoldierView.Shoot;
        }
    }
    [PunRPC]
    void DecreaseIndex()
    {
        if (index > 0)
            index--;
        else
            index = max - 1;
    }
    [PunRPC]
    void IncreaseIndex()
    {
        if (index < max - 1)
            index++;
        else index = 0;
    }
    [PunRPC]
    public void DeactivateWeaponsBehaviours()
    {
        
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Weapons>().canShoot = false;
            }
            
            return;
        
    }
    void DisplayWeaponAmmo()
    {
        switch (index)
        {
            case 0:
                GameManager.instance.ammo.text = "infinite ammo";
                break;
            case 1:
                GameManager.instance.ammo.text = $"ammo:{ShotgunBullets}";
                break;
            case 2:
                GameManager.instance.ammo.text = $"ammo: {UziBullets}";
                break;
        }
    }
  
}
