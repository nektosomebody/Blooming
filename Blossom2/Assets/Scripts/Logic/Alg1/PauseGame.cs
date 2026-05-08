using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    public void Pause()
    {
        pausePanel.SetActive(true); 
    }
    public void Continue()
    {
        pausePanel.SetActive(false);
    }
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
