using System;
using System.Collections;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text totalBest;
    [SerializeField] LevelData levelData;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] string levelType = "alg1";
    [SerializeField] ScoreCalculator scoreCalculator;
    private float delay = 6f;

    TimerUI timer;

    void Start()
    {
        levelData.playerWon += OnPlayerWon;
        timer = GetComponent<TimerUI>();
    }

    public void OnPlayerWon(object sender, EventArgs e)
    {
        float current = Mathf.Round(timer.totalSeconds * 100f) / 100f;

        int score = scoreCalculator != null ? scoreCalculator.CalculateAndSave(current) : 0;

        string uid = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (uid == null || DatabaseConnection.Instance == null)
        {
            totalBest.text = $"Score: {score}\nTotal time: {current}";
            StartCoroutine(ShowGameOverWithDelay());
            return;
        }

        DatabaseConnection.Instance.SaveBestTime(uid, levelType, current, () =>
        {
            DatabaseConnection.Instance.LoadBestTime(uid, levelType, best =>
            {
                totalBest.text = $"Score: {score}\nTotal time: {current}\nBest time: {best}";
                StartCoroutine(ShowGameOverWithDelay());
            });
        });
    }

    private IEnumerator ShowGameOverWithDelay()
    {
        yield return new WaitForSeconds(delay);
        gameOverPanel.SetActive(true);
    }

    public void ToQuit() => SceneManager.LoadScene("MainMenu");
    public void ToContinue() => SceneManager.LoadScene("Game");
}
