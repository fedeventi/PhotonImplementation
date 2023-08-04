using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attack<T> : State<T>
{
    ZombieModel _source;
    
    // Start is called before the first frame update
    public Attack( ZombieModel source)
    {
        _source = source;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        //_source.animator.SetTrigger("Attack");
        _source.photonView.RPC("AttackView",Photon.Pun.RpcTarget.All);
        var targetPosition=_source.target.transform.position;
        targetPosition.y = _source.transform.position.y;
        var direction = (targetPosition - _source.transform.position).normalized;
        _source.transform.rotation= Quaternion.LookRotation(direction);
        //_source.StartCoroutine(AttackTrigger());
    }
    public override void OnUpdate()
    {
        
    }
    IEnumerator AttackTrigger()
    {
        yield return new WaitForSeconds(1.5f);
        _source.attackTrigger.enabled = true;
        yield return new WaitForSeconds(1f); 
        _source.attackTrigger.enabled = false;
        _source.TransitionFsm(States.chase);
    }
    public override void OnSleep()
    {
        base.OnSleep();
    }
}
