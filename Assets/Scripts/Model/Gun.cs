using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using DPR;

//UPDATE `accounts` SET `uzibullets`='1000',`shotgunbullets`='1000' WHERE 1

public class Gun : Weapons
{
    public TrailRenderer lineRenderer;
    public float precision;

    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        if (!canShoot) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (usePhoton)
            {
                
                photonView.RPC("Shoot", RpcTarget.All,UserData.userID);
            }
            else
            {
                Shoot();
            }
        }

    }

    
    [PunRPC]
    public override void Shoot(int userID)
    {
        Debug.Log("DISPARO");

        if (_timeSinceShoot >0 || ammo<1) return;
        
        _timeSinceShoot = 0.01f;

        var direction = transform.forward * 100;
        direction.y = origin.position.y;
       
        float jitter = Random.Range(-precision, precision);
        RaycastHit hit;
        if(shootEvent!=null)
            shootEvent.Invoke();
        if (Physics.Raycast(origin.position, direction + origin.right * jitter, out hit,100,mask))
        {
            if (usePhoton)
            {
                Debug.Log("<color=green>DISPARO PHOTON</color>");
                TrailRenderer trail = Instantiate(lineRenderer, origin.position, Quaternion.identity);
                //TrailRenderer trail = PhotonNetwork.Instantiate("Trail", origin.position, Quaternion.identity).GetComponent<TrailRenderer>();
                StartCoroutine(Trail(trail, hit));
                if (hit.transform)
                {
                    if(photonView.IsMine)
                    {
                        ZombieModel zombie = hit.transform.GetComponent<ZombieModel>();
                        int[] test = new int[] { Damage, userID };
                        if (zombie != null) zombie.photonView.RPC("SendTakeDamage", RpcTarget.MasterClient, test);
                    }
                }
            }
            else
            {
                Debug.Log("<color=red>DISPARO NO PHOTON</color>");
                TrailRenderer trail = Instantiate(lineRenderer, origin.position, Quaternion.identity);
                StartCoroutine(Trail(trail, hit));
            }
        }

    }


    [PunRPC]
    public void StartCoroutineRPC(TrailRenderer trail, RaycastHit hit)
    {
        StartCoroutine(Trail(trail,hit));
    }
    IEnumerator Trail(TrailRenderer trail, RaycastHit hit)
    {
        Debug.Log("DISPARO StartCoroutine");
        float time=0;
        Vector3 startPosition=trail.transform.position;
        while (time < 1)
        {
            trail.transform.position=Vector3.Lerp(startPosition,hit.point,time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position=hit.point;
        Destroy(trail.gameObject, trail.time);

        while (_timeSinceShoot < Cadence)
        {
            _timeSinceShoot += Time.deltaTime;
            yield return null;
        }
        _timeSinceShoot = 0;
        //if (hit.transform)
        //{
        //    if(PhotonNetwork.LocalPlayer== ServerManager.Instance._server)
        //    {
        //        ZombieModel zombie = hit.transform.GetComponent<ZombieModel>();
        //        if (zombie != null) zombie.photonView.RPC("TakeDamage",RpcTarget.All,Damage);
        //    }
        //}
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }
}
