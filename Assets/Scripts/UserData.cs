using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DPR;

namespace DPR
{
    public static class UserData 
    {

        public static string userName;
        public static string nickName;
        public static int userID;
        public static int score;
        public static int coins;
        public static int shotgunBullets;
        public static int uziBullets;
    }


    public static class GameData
    {
        public static string[] RankingTxtName;
        public static string[] RankingTxtScore;

    }


}