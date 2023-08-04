using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SpawnManager : MonoBehaviourPun
{

    private ZombieModel zombie;
    bool usePhoton;
    float _timer;

    public GameObject obj;
    public Transform[] spawnPoints;
    public SoldierModel tgt;
    public TMP_Text roomTxt;
    public TMP_Text debugTxt;
    public TMP_Text msjTxt;
    public float timeToInstantiate=1;    
    public List<SoldierModel> All_soldiers;

    public static SpawnManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    private void Start()
    {

        #if PHOTON_UNITY_NETWORKING
                usePhoton = true;
        #endif

                if (roomTxt)
                    roomTxt.text = $"{PhotonNetwork.LocalPlayer.NickName}, Nro: {PhotonNetwork.LocalPlayer.ActorNumber} \n" +
                        $"MasterClient: {PhotonNetwork.IsMasterClient}";

       
    }

    bool play;

    private void Update()
    {
        if (!ServerManager.Instance.AllPlayersInRoom && play == false)
        {
            msjTxt.text = $"ESPERANDO A LOS JUGADORES {PhotonNetwork.CurrentRoom.Players.Count-1} / {4}";
            play = true;
            return;
        }
        else
        {
            msjTxt.text = "";
            msjTxt.gameObject.SetActive(false);
        }


        if (usePhoton)
        {            
            

            if (PhotonNetwork.LocalPlayer == ServerManager.Instance._server)
            {
                if (_timer < timeToInstantiate)
                    _timer += Time.deltaTime;
                else
                {
                    var rnd = Random.Range(0, spawnPoints.Length);
                    photonView.RPC("SpawnZombie", RpcTarget.MasterClient, rnd);
                    _timer = 0;
                   
                }
            }
        }

    }


    [PunRPC]
    private void SpawnZombie(int spawns)
    {
        var lSoldiers = FindObjectsOfType<SoldierModel>();
        All_soldiers = new List<SoldierModel>(lSoldiers);
        if (All_soldiers.Count <= 0) return;

        tgt = All_soldiers[Random.Range(0, All_soldiers.Count)];
        zombie = PhotonNetwork.Instantiate(obj.name, spawnPoints[spawns].position, Quaternion.identity).GetComponent<ZombieModel>();
       
    }

  
    private void SpawnZombieLocal(int spawns)
    {        
        zombie = Instantiate(obj, spawnPoints[spawns].position, Quaternion.identity, transform.GetChild(1)).GetComponent<ZombieModel>();
        zombie.transform.GetChild(0).GetChild(0).GetChild(Random.Range(2, zombie.transform.GetChild(0).GetChild(0).childCount - 1)).gameObject.SetActive(true);
        zombie.transform.parent = transform.GetChild(1);
        zombie.GetComponent<ZombieModel>().target = tgt.transform;
        zombie.GetComponent<Flocking>().leaderToFollow = tgt.transform;
    }
}





