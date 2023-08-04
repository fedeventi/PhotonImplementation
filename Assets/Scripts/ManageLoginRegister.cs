using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageLoginRegister : MonoBehaviour
{
    public GameObject login, register;

    private void Start()
    {
        login.SetActive(true);
        register.SetActive(false);
    }
    public void SwitchMenu()
    {
        if (login.activeInHierarchy)
        {
            login.SetActive(false);
            register.SetActive(true);
        }
        else
        {
            login.SetActive(true);
            register.SetActive(false);
        }
    }

}
