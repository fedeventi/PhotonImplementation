using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    public float range;
    public LayerMask mask;
    public Transform leaderToFollow;
    Vector3 _dir;
    public float separationWeight;
    public float leaderWeight;
    public float cohesionWeight;
    public float alineationWeight;


    public void Start()
    {      
        mask = (int)Mathf.Pow(2, gameObject.layer);
    }
    public void SetLeader()
    {
        if (leaderToFollow)
        {
            
        }
        else
        {
            
        }
        
        
    }
    public void ClearTarget()
    {
        if (leaderToFollow)
        {

          
          


        }
    }
    public Vector3 GetDir()
    {
        Collider[] entities = Physics.OverlapSphere(transform.position, range, mask);
        var x = leaderToFollow.position;
        var separation = GetSeparationDir(transform.position, entities, range) * separationWeight;
        var alineation = GetAlineationDir(transform.position, entities, range) * alineationWeight;
        var cohesion = GetCohesionDir(transform.position, entities) * cohesionWeight;
        var leader = GetLeaderDir(transform.position, leaderToFollow.transform.position) * leaderWeight;

        _dir = (separation + cohesion + leader + alineation).normalized;
        
        return _dir;
    }
    
    Vector3 GetSeparationDir(Vector3 origin, Collider[] entities, float maxDistance)
    {
        Vector3 separation = Vector3.zero;
        for (int i = 0; i < entities.Length; i++)
        {
            var curr = new Vector3(entities[i].transform.position.x,transform.position.y, entities[i].transform.position.z);
            float multiplier = maxDistance - Vector3.Distance(curr, origin);
            separation += (origin - curr).normalized * multiplier;
        }
        return separation.normalized;
    }
    
    Vector3 GetAlineationDir(Vector3 origin, Collider[] entities, float maxDistance)
    {
        Vector3 averageDir = Vector3.zero;
        for (int i = 0; i < entities.Length; i++)
        {
            var curr = entities[i];
            float distance = Vector3.Distance(new Vector3(curr.transform.position.x,transform.position.y,curr.transform.position.z), origin);
            float multiplier = (maxDistance - distance) / maxDistance;
            averageDir += curr.transform.forward * multiplier;
        }
        return averageDir.normalized;
    }
    
    Vector3 GetCohesionDir(Vector3 origin, Collider[] entities)
    {
        Vector3 averagePos = Vector3.zero;
        for (int i = 0; i < entities.Length; i++)
        {
            averagePos += new Vector3(entities[i].transform.position.x,transform.position.y,entities[i].transform.position.z);
        }
        averagePos /= entities.Length;
        return (averagePos - origin).normalized;
    }
    
    Vector3 GetLeaderDir(Vector3 origin, Vector3 target)
    {
        return (new Vector3(target.x,origin.y,target.z) - origin).normalized;
    }
    
}
