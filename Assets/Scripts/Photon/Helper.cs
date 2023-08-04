using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPR
{
    public class Helper : MonoBehaviour
    {

        public static string GetData(string data, string index)
        {
            string value = data.Substring(data.IndexOf(index) + index.Length);
            if (value.Contains("|")) value = value.Remove(value.IndexOf('|'));
            return value;
        }

    }

}

