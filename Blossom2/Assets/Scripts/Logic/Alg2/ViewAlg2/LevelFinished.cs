using UnityEngine;

public class LevelFinished : MonoBehaviour
{
    [SerializeField] ScoreCalculator scoreCalculator;
    [SerializeField] TimerUI timer;
    [SerializeField] GameObject victoryPanel;

    public void Init(LevelResultManager manager)
    {
        manager.OnWin += OnWin;
    }

    void OnWin()
    {
        timer.StopTimer();

        if (scoreCalculator != null && timer != null)
            scoreCalculator.CalculateAndSave(timer.totalSeconds);

        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }
}
