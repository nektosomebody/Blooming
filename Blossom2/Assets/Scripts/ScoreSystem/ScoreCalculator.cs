using System;
using Firebase.Auth;
using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] GameObject timer;
    [SerializeField] TMP_Text textForScore;
    [SerializeField] float multiplier = 1000;
    [SerializeField] string levelType = "alg1";
    private float bestScore = -1f;

    async void SetResult(object sender, EventArgs eventArgs)
    {
        int result = CalcResult(timer.GetComponent<TimerUI>().totalSeconds);
        textForScore.text = $"Your score is: {result}";

        string uid = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (uid != null)
            await DatabaseConnection.Instance.SaveScoreAsync(uid, levelType, result);
    }

    int CalcResult(float seconds)
    {
        return Mathf.RoundToInt(seconds / bestScore * multiplier);
    }
}
