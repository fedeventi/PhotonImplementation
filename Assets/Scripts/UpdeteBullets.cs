using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using DPR;
using UnityEngine.SceneManagement;
using TMPro;

public class UpdeteBullets : MonoBehaviour
{


    public void UpdateBullet()
    {
        StartCoroutine("ProcessUpdateBulletRequest");
    }

    private IEnumerator ProcessUpdateBulletRequest()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post(WebServer.SERVER_NAME + "/updateBullets.php", form);
        yield return www.SendWebRequest();
    }

}
