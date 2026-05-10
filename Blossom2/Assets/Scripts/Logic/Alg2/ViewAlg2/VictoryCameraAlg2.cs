using UnityEngine;

public class VictoryCameraAlg2 : MonoBehaviour
{
    public float radius = 15f;
    public float targetHeight = 50f;
    public float rotationSpeed = 30f;

    private bool isVictory = false;
    private float angle = 0f;
    private Vector3 levelCenter;

    void Update()
    {
        if (!isVictory) return;

        angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        transform.position = levelCenter + new Vector3(x, targetHeight, z);
        transform.LookAt(levelCenter + Vector3.up * 10f);
    }

    public void PlayVictoryAnimation(Vector3 center)
    {
        isVictory = true;
        levelCenter = center;
        Debug.Log($"Victory Camera: starting rotation around {levelCenter}");
    }

    public void StopVictoryAnimation()
    {
        isVictory = false;
        Debug.Log("Victory Camera: stopping rotation");
    }
}
