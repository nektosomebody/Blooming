using System;
using System.Collections;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text totalBest;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] string levelType = "alg1";
    [SerializeField] LevelData levelData;
    [SerializeField] TimerUI timer;
    ScoreCalculator scoreCalculator;
    private float delay = 6f;

    

    void Start()
    {
        scoreCalculator = GetComponent<ScoreCalculator>();
        levelData.playerWon += OnPlayerWon;
    }

    public void OnPlayerWon(object sender, EventArgs e)
    {
        timer.StopTimer();
        float current = Mathf.Round(timer.totalSeconds * 100f) / 100f;

        int score = scoreCalculator != null ? scoreCalculator.CalculateScore(current) : 0;

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
    public void ToContinue() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
