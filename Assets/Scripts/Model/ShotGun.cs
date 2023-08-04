using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using DPR;

public class ShotGun : Weapons
{
    public TrailRenderer lineRenderer;

    public override void Start()
    {
        if (!photonView.IsMine) return;
        base.Start();
        ammo = UserData.shotgunBullets;
        photonView.RPC("UpdateBullet", RpcTarget.OthersBuffered, ammo);
    }


    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (usePhoton)
            {
                photonView.RPC("Shoot", RpcTarget.All, UserData.userID);
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
        if (!canShoot) return;
        if (_timeSinceShoot > 0 || ammo < 1) return;
        _timeSinceShoot = 0.01f;

        if (shootEvent != null)
            shootEvent.Invoke();
        if (ammo > 0)
        {
            ammo--;
            if (userID == UserData.userID)
                StartCoroutine(DiscountBullets(1, 2, userID));
        }
        var direction = transform.forward * 100;
        direction.y = origin.position.y;
        
        
        RaycastHit hit;
        for (int i = -2; i < 3; i++)
        {

            if (Physics.Raycast(origin.position, Quaternion.AngleAxis(i * 5, transform.up) *direction, out hit, 100, mask))
            {
                TrailRenderer trail = Instantiate(lineRenderer, origin.position, Quaternion.identity);
                StartCoroutine(Trail(trail, hit));
                if (hit.transform)
                {
                    int[] test = new int[] { Damage, userID };
                    if (photonView.IsMine)
                    {
                        ZombieModel zombie = hit.transform.GetComponent<ZombieModel>();
                        if (zombie != null) zombie.photonView.RPC("SendTakeDamage", RpcTarget.MasterClient, test);
                    }
                }
            }
        }
    }
   

    IEnumerator Trail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
        while (_timeSinceShoot < Cadence)
        {
            _timeSinceShoot += Time.deltaTime;
            yield return null;
        }
        _timeSinceShoot = 0;
        //if (hit.transform)
        //{
        //    if (PhotonNetwork.LocalPlayer == ServerManager.Instance._server)
        //    {
        //        ZombieModel zombie = hit.transform.GetComponent<ZombieModel>();
        //        if (zombie != null) zombie.photonView.RPC("TakeDamage", RpcTarget.All, Damage);
        //    }
        //}

    }

    IEnumerator DiscountBullets(int bullets, int id, int userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", userID);
        form.AddField("price", 0);
        form.AddField("cantBullets", -bullets);
        form.AddField("weapponID", id);

        Debug.Log("--> 1");
        UnityWebRequest www = UnityWebRequest.Post(WebServer.SERVER_NAME + "/updateBullets.php", form);
        yield return www.SendWebRequest();
        Debug.Log("--> 2");

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log($"Server error, please retry later.\n\n" +
                $"{www.error}");


            yield return null;
        }
        else
        {
            Debug.Log("-->" + www.downloadHandler.text);
            if (Helper.GetData(www.downloadHandler.text, "MESSAGE:") == "buy successfully!")
            {
                Debug.Log("-->" + www.downloadHandler.text);
                UserData.coins = int.Parse(Helper.GetData(www.downloadHandler.text, "COINS:"));
                UserData.uziBullets = int.Parse(Helper.GetData(www.downloadHandler.text, "USZIBULLETS:"));
                UserData.shotgunBullets = int.Parse(Helper.GetData(www.downloadHandler.text, "SHOTGUNBULLETS:"));
            }

        }
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }
}
