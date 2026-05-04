using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Assets.Scripts.ScoreSystem;



public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] GameObject timer;
    [SerializeField] TMP_Text textForScore;
    [SerializeField] float multiplier = 1000;
    private float bestScore = -1f;

    DatabaseConnection dbConnnect = new();

    void SetResult(object sender, EventArgs eventArgs)
    {
        int result = CalcResult(timer.GetComponent<TimerUI>().totalSeconds);
        textForScore.text = $"Your score is: {result}";
        dbConnnect.AddNewScore("test", result);
    }

    int CalcResult(float seconds)
    {
        return Mathf.RoundToInt(seconds / bestScore * multiplier);
    }
}

