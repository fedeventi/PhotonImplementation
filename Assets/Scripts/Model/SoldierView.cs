using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SoldierView : MonoBehaviour
{

    Animator animator;
    Image hurtImage;
    public Slider life;
    void Start()
    {
        animator = GetComponent<Animator>();
        hurtImage= ServerManager.Instance.hurtImage;
    }


    public void TakeDamage()
    {

        hurtImage.GetComponent<Animator>().SetTrigger("hurt");
    }
    public void WalkAnimation(Vector3 dir)
    {
        animator.SetFloat("speed", Mathf.Abs(dir.normalized.magnitude));

    }
    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }
    public void Death()
    {
        if(animator != null)
            animator.SetBool("Death",true);
        hurtImage.GetComponent<Animator>().SetBool("dead",true);
    }
    public void UpdateLife(float soldierLife)
    {
        life.value = soldierLife / 100;
    }
}
