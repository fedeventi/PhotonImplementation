using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Photon.Pun;
using System;
using System.Linq;

public class ZombieModel : MonoBehaviourPun
{

    public int life=100;
    StateMachine<States> _fsm;
    public Transform target;
    Dictionary<States, State<States>> _statesMap=new Dictionary<States, State<States>>();
    public float obstacleAvoidance;
    public LayerMask walls;
    public float speed;
    public Animator animator;
    public Collider attackTrigger;
    bool usePhoton;

    public int killID;

    void Start()
    {
        #if PHOTON_UNITY_NETWORKING
                usePhoton = true;
        #endif

        animator = GetComponent<Animator>();
        SetStates();
       

        transform.GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(true);
        if (usePhoton)
        {
            if (photonView.IsMine)
            {
                SetTarget();
                GetComponent<Flocking>().leaderToFollow = target;
            }
        }

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Debug.Log("soy el server");
            _fsm.OnUpdate();
            if (target != null)
            {
                if (Vector3.Distance(target.position, transform.position) > 10 )
                {
                    SetTarget();
                    GetComponent<Flocking>().leaderToFollow = target;

                }
            }

        }

    }
   
    public void SetStateMachine()
    {
        _fsm = new StateMachine<States>(_statesMap[States.chase]);
    }

    public void TransitionFsm(States state)
    {
        _fsm.Transition(state);
    }

    public void SetStates()
    {
        var chase = new Chase<States>(this);
        var death= new Death<States>(this);
        var attack = new Attack<States>(this);
        _statesMap.Add(States.death, death);
        _statesMap.Add(States.chase, chase);
        chase.SetTransition(States.attack,attack);
        chase.SetTransition(States.death, death);
        attack.SetTransition(States.chase,chase);
        attack.SetTransition(States.death,death);
        SetStateMachine();
        
    }

    [PunRPC]
    public void SendTakeDamage(int[] Damage)
    {       
        photonView.RPC("TakeDamage", RpcTarget.All, Damage);
    }


    [PunRPC]
    public void TakeDamage(int[] Damage)
    {
        if(life>0)
            life -= Damage[0];
        if (life <= 0)
        {
            killID = Damage[1];
            TransitionFsm(States.death);
        }
    }


    public void DestroZombie()
    {
        if(photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void AttackView()
    {
        animator.SetBool("Walking", false);
        animator.SetTrigger("Attack");
        StartCoroutine(EnableAttackCollider());
    }
    
    public void SetTarget()
    {
        if (SpawnManager.Instance.All_soldiers.Count > 0)
        {
            var tgt = SpawnManager.Instance.All_soldiers.Where(soldier => !soldier.dead).OrderBy(soldier => Vector3.Distance(transform.position, soldier.transform.position)).FirstOrDefault();
            if(tgt != null)
                target =tgt.transform;

        }        
    }


    [PunRPC]
    public void Death()
    {  
        animator.SetBool("Dead", true);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (target != null)
            Gizmos.DrawLine(transform.position, target.position);
    }
    public IEnumerator EnableAttackCollider()
    {
        yield return new WaitForSeconds(1.5f);
        attackTrigger.enabled = true;
        yield return new WaitForSeconds(1f);
        attackTrigger.enabled = false;
        TransitionFsm(States.chase);
    }


}
