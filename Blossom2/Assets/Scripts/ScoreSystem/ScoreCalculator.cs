using Firebase.Auth;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] float optimalTime = 60f;
    [SerializeField] float multiplier = 1000f;

    public int CalculateScore(float seconds)
    {
        int result = Mathf.RoundToInt(optimalTime / seconds * multiplier);
        return result;
    }
}