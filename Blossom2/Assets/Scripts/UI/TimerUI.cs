using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerUI : MonoBehaviour
{
    public float totalSeconds { get; private set; } = 0f;
    TMP_Text text;
    [SerializeField] LevelData levelData;
    bool timerIsActive = true;

    void Start()
    {
        totalSeconds = 0;
        text = GetComponentInChildren<TMP_Text>();
        levelData.playerWon += StopTimer;
    }

    void StopTimer(object s, EventArgs e)
    {
        timerIsActive = false;
    }

    public void StopTimer()
    {
        timerIsActive = false;
    }

    public void ContinueTimer()
    {
        timerIsActive = true;
    }

    void Update()
    {
        if (!timerIsActive)
        {
            return;
        }
        totalSeconds += Time.deltaTime;
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);

        text.text = $"Time:\r\n{minutes:00}:{seconds:00}";
    }
}