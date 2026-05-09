using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    TimerUI timer;

    public void Start()
    {
        timer = GetComponent<TimerUI>();
        Debug.LogError("Timer: " + (timer == null));
    }
    public void Pause()
    {
        try {
        pausePanel.SetActive(true);
        timer.StopTimer();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error pausing game: {ex.Message}");
        }
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
