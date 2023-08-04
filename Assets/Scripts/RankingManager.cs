using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DPR;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class RankingManager : MonoBehaviour
{
    float curTime;
    bool inMenu;

    public List<TMP_Text> RankingTxtPos;
    public List<TMP_Text> RankingTxtName;
    public List<TMP_Text> RankingTxtScore;

    public TMP_Text MyNameTxt;
    public TMP_Text MyScoreTxt;
    public TMP_Text MyRanck;

    public string[] rank;
    public List<string> lRanck;


    public static RankingManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {


        GetRanking();
        if (SceneManager.GetActiveScene().name == "Loby")
        {
            inMenu = true;
        }

    }


    private void Update()
    {
        if (inMenu)
        {
            curTime += Time.deltaTime;
            if (curTime > 2f)
            {
                GetRanking();
                curTime = 0;
            }
        }
    }



    public void GetRanking()
    {
        lRanck.Clear();
        StartCoroutine(ProcessGetRankingRequest());
    }

    private IEnumerator ProcessGetRankingRequest()
    {

        WWWForm form = new WWWForm();
        form.AddField("id", UserData.userID);

        UnityWebRequest www = UnityWebRequest.Post(WebServer.SERVER_NAME + "/ranking.php", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log($"Server error, please retry later.\n\n" +
               $"{www.error} \n\n");

            yield return null;
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var f = www.downloadHandler.text.Split(';');
            rank = new string[f.Length];
            for (int i = 0; i < f.Length; i++)
            {
                lRanck.Add(f[i]);
            }

    
            GameData.RankingTxtName = new string[(lRanck.Count > 10 ? 10 : lRanck.Count)];
            GameData.RankingTxtScore = new string[(lRanck.Count > 10 ? 10 : lRanck.Count)];
            var topTen = true;
            for (int i = 0; i < (lRanck.Count > 10 ? 10 : lRanck.Count); i++)
            {
                var value = lRanck[i].Split('|');
               
                GameData.RankingTxtName[i] = value[0];
                GameData.RankingTxtScore[i] = value[1];

                if (RankingTxtScore.Count >= GameData.RankingTxtScore.Length && RankingTxtName.Count >= GameData.RankingTxtScore.Length)
                {
                    var color = value[0] == UserData.userName ? "green" : "white";
                    RankingTxtPos[i].text = $"<color={color}>{i + 1}</color>";
                    RankingTxtName[i].text = $"<color={color}>{value[0]}</color>";
                    RankingTxtScore[i].text = $"<color={color}>{value[1]}</color>";
                }

                if(value[0] == UserData.userName)
                {
                    topTen = false;
                }
            }

            if (MyNameTxt != null && MyScoreTxt != null && MyRanck != null)
            {

                for (int i = 0; i < lRanck.Count; i++)
                {
                    var value = lRanck[i].Split('|');


                    if (value[0] == UserData.userName)
                    {
                        MyRanck.text = $"<color=green>{i + 1 }</color>";
                        MyNameTxt.text = $"<color=green>{value[0]}</color>";
                        MyScoreTxt.text = $"<color=green>{value[1]}</color>";
                    }
                }

                if (!topTen)
                {
                    MyRanck.text = $"";
                    MyNameTxt.text = $"";
                    MyScoreTxt.text = $"";
                }
            }


            if (ServerManager.Instance != null)
            {
                if (ServerManager.Instance.Wining)
                {
                    ServerManager.Instance.GoToMenu();
                }
            }

        }

    }

}
