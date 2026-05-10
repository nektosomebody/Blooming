using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public float totalSeconds { get; private set; } = 0f;
    bool timerIsActive = true;


    public void StopTimer()
    {
        timerIsActive = false;
        // Time.timeScale = 0f;
        Debug.Log($"Timer stopped at {totalSeconds} seconds.");
    }

    public void ContinueTimer()
    {
        timerIsActive = true;
        // Time.timeScale = 1f;
    }

    public void ResetTimer()
    {
        totalSeconds = 0f;
        timerIsActive = true;
        text.text = "Time:\r\n00:00";
        Debug.Log("Timer reset");
    }

    void Update()
    {
        if (!timerIsActive) return;

        totalSeconds += Time.deltaTime;
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        text.text = $"Time:\r\n{minutes:00}:{seconds:00}";
    }
}