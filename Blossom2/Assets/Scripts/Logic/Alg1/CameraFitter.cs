using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitter : MonoBehaviour
{
    public float padding = 1f;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void FitCamera(float width, float height, Vector3 center)
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float levelRatio = width / height;

        float size;
        if (screenRatio >= levelRatio)
        {
            size = height / 2f;
        }
        else
        {
            size = width / screenRatio / 2f;
        }
        cam.orthographicSize = size + padding;
        transform.position = new Vector3(center.x, size + padding, center.z);
        
        /*
        float aspect = (float)Screen.width / Screen.height;
        float halfFov = cam.fieldOfView * 0.5f * Mathf.Deg2Rad;

        float halfW = width / 2f + padding;
        float halfH = height / 2f + padding;

        float distForHeight = halfH / Mathf.Tan(halfFov);
        float distForWidth = halfW / aspect / Mathf.Tan(halfFov);

        float dist = Mathf.Max(distForHeight, distForWidth);

        Debug.LogWarning($"Distance: {dist}, y = {center.y}, padding = {padding}");
        transform.position = new Vector3(center.x, dist + padding, center.z);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        */
    }

    public void FitCamera(Transform target)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();


        if (renderers.Length == 0)
        {
            Debug.LogError("No renderers found in target");
            return;
        }

        Bounds bounds = renderers[0].bounds;

        foreach (var rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }
        
        Vector3 center = bounds.center;
        float width = bounds.size.x;
        float height = bounds.size.z;

        FitCamera(width, height, center);
    }
}