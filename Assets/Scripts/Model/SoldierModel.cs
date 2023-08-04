using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DPR;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class SoldierModel : MonoBehaviourPun
{

    Rigidbody rb;
    Animator anim;

    public int life;
    public float speed;
    public float jumpForce;
    public bool dead;
    SoldierView _soldierView;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (photonView.IsMine)
        {
            var cam = FindObjectOfType<CameraController>();
            cam.character = gameObject;
            cam.SetValues();
            _soldierView = GetComponent<SoldierView>();
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            _soldierView.life = GameManager.instance.lifeBar;
            
        }
    }


    bool updata = false;
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (dead)
            {
                SpawnManager.Instance.All_soldiers.Remove(this);
                ServerManager.Instance.deadPanel.SetActive(true);
                if (!updata)
                {
                    StartCoroutine(UploadData(UserData.score,UserData.coins));
                    RankingManager.Instance.GetRanking();
                    updata = true;
                }
            }

            if (ServerManager.Instance.EndGame)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!updata)
                    {
                        updata = true;
                        ServerManager.Instance.Wining = true;
                        StartCoroutine(UploadData(UserData.score, UserData.coins));
                        RankingManager.Instance.GetRanking();
                    }
                }

            }

        }
    }
    public void Move(Vector3 dir)
    {
        if (dead) return;
        var velocity = dir.normalized * speed ;
        rb.velocity = velocity;
        _soldierView.WalkAnimation(dir);
    }
    public void LookAt(Vector3 position)
    {
        if (dead) return;
        transform.LookAt(position);
    }
    

    public Vector3 mousePos()
    {
        
        var pos = CameraController.CheckMousePosition();
        return new Vector3(pos.x, transform.position.y, pos.z);
    }

    IEnumerator UploadData(int score,int coins)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", UserData.userID);
        form.AddField("score", score);
        form.AddField("coins", coins);
        UnityWebRequest www = UnityWebRequest.Post(WebServer.SERVER_NAME + "/updateData.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log($"Server error, please retry later.\n\n" +
                $"{www.error}");


            yield return null;
        }
        else
        {
            if (www.downloadHandler.text == "Update successfully!")
            {
                Debug.Log("Update successfully!");
                UserData.coins = int.Parse(Helper.GetData(www.downloadHandler.text, "COINS:"));
                UserData.score = int.Parse(Helper.GetData(www.downloadHandler.text, "SCORE:"));
                //RankingManager.Instance.GetRanking();
            }       

        }
    }


    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (dead) return;
        if (life > 0)
        {
            life -= damage;
            if (_soldierView)
                _soldierView.TakeDamage();

        }
        if (life <= 0)
        {
            
            photonView.RPC("Death", RpcTarget.All);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos(), 0.5f);
    }
    [PunRPC]
    void Death()
    {
        if(_soldierView)
            _soldierView.Death();
        dead = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    public SoldierView SoldierView => _soldierView;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Attack")
        {
            if (photonView.IsMine)
            {
                photonView.RPC("TakeDamage", RpcTarget.All, 10);
                other.enabled = false;
            }

            
        }
    }


    public void Win()
    {
        if (!updata)
        {
            StartCoroutine(UploadData(UserData.score, UserData.coins));
            RankingManager.Instance.GetRanking();
            updata = true;
            Debug.LogError("WIN");
        }
    }

}
