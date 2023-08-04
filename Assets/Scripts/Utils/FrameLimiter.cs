using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    public int frameTarget = 60;
    [Range(-1,4)]
    public int vSync = 0;

    public bool limitFPS;

    private void Awake()
    {
        if (limitFPS) 
        {
            #if UNITY_EDITOR
                QualitySettings.vSyncCount = vSync;
                Application.targetFrameRate = frameTarget;
            #endif
        }
    }

}
