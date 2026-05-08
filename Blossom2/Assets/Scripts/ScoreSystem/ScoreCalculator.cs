using Firebase.Auth;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] float optimalTime = 60f;
    [SerializeField] float multiplier = 1000f;
    [SerializeField] string levelType = "alg1";

    public int CalculateAndSave(float seconds)
    {
        int result = Mathf.RoundToInt(optimalTime / seconds * multiplier);

        string uid = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        DatabaseConnection.Instance.SaveScore(uid, levelType, result);

        return result;
    }
}