using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DPR;

public class GameManager : MonoBehaviour
{
    int _points;
    int _coins;
    int _killToCoin=1;
    public static GameManager instance;
    public Text pointText;
    public Text coinText;
    public Slider lifeBar;
    public Text ammo;

    public int Score
    {
        get
        {
            return _coins;
        }
    }

    private void Awake()
    {
        instance = this;
    }


    void Update()
    {
        if (pointText)
            pointText.text = $"puntos: {_points}";
        if (coinText)
            coinText.text = $"monedas: {_coins}";
    }


    public void AddPoints(int a,int id)
    {

        Debug.Log($"id: {id}");
        if (id == UserData.userID)
        {
            _points += 10;
            UserData.score = _points;
            CalculateCoins();
            UserData.coins = _coins;
        }
    }
    public void CalculateCoins()
    {
        if (_killToCoin < 10)
            _killToCoin++;
        else
        {
            _killToCoin = 1;
            _coins+=10;
        }
        


    }


    public void Quit()
    {
        Application.Quit();
    }

}
