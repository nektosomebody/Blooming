using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] TimerUI timer;

    public void Start()
    {
        Debug.LogError("Timer: " + (timer == null));
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        timer.StopTimer();
    }
    public void Continue()
    {
        pausePanel.SetActive(false);
        timer.ContinueTimer();
    }
    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
