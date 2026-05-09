using System;
using UnityEngine;

public class LevelFinished : MonoBehaviour
{
    [SerializeField] ScoreCalculator scoreCalculator;
    [SerializeField] TimerUI timer;
    [SerializeField] GameObject victoryPanel;

    public void Init(LevelResultManager manager)
    {
        manager.playerWon += OnWin;
    }

    void OnWin(object s, EventArgs args)
    {
        timer.StopTimer();

        if (scoreCalculator != null && timer != null)
            scoreCalculator.CalculateScore(timer.totalSeconds);

        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }
}
