using Photon.Pun;
using UnityEngine;

public class SoldierController : MonoBehaviourPun
{
    SoldierModel model;
    Vector3 dir;

    private void Awake()
    {
        model = GetComponent<SoldierModel>();
    }


    void Update()
    {
        if (!photonView.IsMine) return;
        model.SoldierView.UpdateLife(model.life);
        model.LookAt(model.mousePos());
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        var yAxis = Input.GetAxis("Vertical");
        var xAxis = Input.GetAxis("Horizontal");
        dir = (Vector3.forward * yAxis) + Vector3.right * xAxis;        
        model.Move(dir);        
    }
}
