using System;
using System.Collections;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text totalBest;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] string levelType = "alg1";
    [SerializeField] LevelData levelData;
    [SerializeField] TimerUI timer;
    [SerializeField] VictoryCameraAlg2 victoryCamera;
    [SerializeField] Button pauseButton;
    ScoreCalculator scoreCalculator;
    private float delay = 6f;

    

    void Start()
    {
        scoreCalculator = GetComponent<ScoreCalculator>();
        levelData.playerWon += OnPlayerWon;
    }

    public void OnPlayerWon(object sender, EventArgs e)
    {
        if (pauseButton != null)
            pauseButton.interactable = false;

        if (levelType == "alg2")
        {
            int completedLevels = PlayerPrefs.GetInt("alg2_levels_completed", 0);
            PlayerPrefs.SetInt("alg2_levels_completed", completedLevels + 1);
            PlayerPrefs.Save();
            Debug.Log($"Alg2 levels completed: {completedLevels + 1}, Vertex count next: {6 + (completedLevels + 1) / 3}");
        }

        if (levelData is LevelResultManager resultManager)
        {
            resultManager.PlayTargetVictoryAnimations();

            if (victoryCamera != null)
            {
                Vector3 levelCenter = resultManager.GetLevelCenter();
                victoryCamera.PlayVictoryAnimation(levelCenter);
            }
        }

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

        DatabaseConnection.Instance.SaveScore(uid, levelType, score, () =>
        {
            DatabaseConnection.Instance.SaveBestTime(uid, levelType, current, () =>
            {
                DatabaseConnection.Instance.LoadBestTime(uid, levelType, best =>
                {
                    totalBest.text = $"Score: {score}\nTotal time: {current}\nBest time: {best}";
                    StartCoroutine(ShowGameOverWithDelay());
                });
            });
        });
    }

    private IEnumerator ShowGameOverWithDelay()
    {
        yield return new WaitForSeconds(delay);
        gameOverPanel.SetActive(true);
    }

    public void ToQuit()
    {
        if (pauseButton != null)
            pauseButton.interactable = true;
        if (victoryCamera != null)
            victoryCamera.StopVictoryAnimation();
        SceneManager.LoadScene("MainMenu");
    }

    public void ToContinue()
    {
        if (pauseButton != null)
            pauseButton.interactable = true;
        if (victoryCamera != null)
            victoryCamera.StopVictoryAnimation();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
