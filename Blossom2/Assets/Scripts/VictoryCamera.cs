using UnityEngine;
using System;

public class VictoryCamera : MonoBehaviour
{
    public Transform target;
    public float radius = 5f;
    public float targetHeight = 200f;
    public float rotationSpeed = 50f;
    public float dropSpeed = 2f;
    public float heightSpeed = 2f;

    private bool victory = false;
    private float angle = 0f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!victory) return;
        angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(target.position.x + x, transform.position.y, target.position.z + z);

        transform.LookAt(target.transform.position);
    }

    public void OnPlayerWon()
    {
        victory = true;
        GetComponent<Camera>().orthographic = false;
    }
}