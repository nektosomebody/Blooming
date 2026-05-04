using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text TotalBest;
    [SerializeField] LevelData levelData;
    [SerializeField] GameObject gameOverPanel;
    TimerUI timer;
    void Start()
    {
        levelData.playerWon += OnPlayerWon;
        if (PlayerPrefs.GetFloat("SavedBestScore") == 0)
        {
            PlayerPrefs.SetFloat("SavedBestScore", 10000000);
        }
        timer = GetComponent<TimerUI>();
    }

    public void OnPlayerWon(object sender, EventArgs e)
    {
        BestScoreUpdate();
        StartCoroutine(ShowGameOverWithDelay());
    }

    private IEnumerator ShowGameOverWithDelay()
    {
        yield return new WaitForSeconds(6f);
        gameOverPanel.SetActive(true);
    }
    public void BestScoreUpdate()
    {
        if (PlayerPrefs.HasKey("SavedBestScore"))
        {
            if (timer.totalSeconds < PlayerPrefs.GetFloat("SavedBestScore"))
            {
                PlayerPrefs.SetFloat("SavedBestScore", Mathf.Round(timer.totalSeconds * 100f) / 100f);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("SavedBestScore", Mathf.Round(timer.totalSeconds * 100f) / 100f);
        }
        TotalBest.text = $"Total time: {Mathf.Round(timer.totalSeconds * 100f) / 100f}\nBest time: {PlayerPrefs.GetFloat("SavedBestScore")}";
    }
    public void ToQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ToContinue()
    {
        SceneManager.LoadScene("Game");
    }
}
