using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Chase<T> : State<T>
{
    ZombieModel _source;
    TargetDetection _tgtDetect;
    Flocking _flocking;
    public Chase(ZombieModel source)
    {
        _source = source;
        _tgtDetect = _source.GetComponent<TargetDetection>();
        _flocking = _source.GetComponent<Flocking>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _source.animator.SetBool("Walking", true);
        //if(_source.target == null)
        //{
        //    if (_source.usePhoton)
        //    {
        //        _source.photonView.RPC("SetTarget", RpcTarget.All);
        //        _source.GetComponent<Flocking>().leaderToFollow = _source.target;
        //    }
        //}
    }
    public override void OnUpdate()
    {
        
        base.OnUpdate();
        if (!_source.target) return;
        var directionToTarget = (_source.target.transform.position - _source.transform.position);
        var distance = directionToTarget.magnitude;
        if (distance < 2) _source.TransitionFsm(States.attack);
        Vector3 dir=directionToTarget;
        
        if (_tgtDetect.MyClosestObstacle())
        {
            

            dir = (_tgtDetect.MyClosestPointToTarget(_source.target.transform.position) - _source.transform.position);

        }
        else
        {
            dir = directionToTarget;

        }
        dir += _flocking.GetDir();
        dir.y = 0;
        _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 2f * Time.deltaTime);

        _source.transform.position += _source.transform.forward * _source.speed * 2f * Time.deltaTime;
    }
    public override void OnSleep()
    {
        base.OnSleep();
        _source.animator.SetBool("Walking", false);
    }
}
