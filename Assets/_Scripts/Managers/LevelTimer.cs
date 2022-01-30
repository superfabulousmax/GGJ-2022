using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class LevelTimer : MonoBehaviour
{
    private float timer;
    private bool isGamerOver;
    void Start()
    {
        timer = 0;
        isGamerOver = false;
    }

    void Update()
    {
        if (isGamerOver)
            return;
        var time = TimeSpan.FromSeconds(Time.realtimeSinceStartup);
        if(time.Minutes >= Constants.GameLengthInMinutes)
        {
            GamePlayManager.Instance.SendGameOver();
            isGamerOver = true;
        }
    }
}
