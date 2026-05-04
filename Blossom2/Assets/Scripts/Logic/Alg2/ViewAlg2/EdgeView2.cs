using Alg2.Domains;
using Unity.VisualScripting;
using UnityEngine;


public class EdgeView2 : MonoBehaviour
{

    EdgeWithFlow edge;
    public int Capacity { get; private set; }
    public int CurFlow { get; private set; }
    float edgeLength;
    Vector2 swipeStart;
    bool isSwiping;
    GameObject flowInstance;
    Vector3 startPos;

    const float SwipeThreshold = 15f;

    public void Init(EdgeWithFlow e, float edgeLen, GameObject flInstance)
    {
        edge = e;
        Capacity = edge.Cap;
        edgeLength = edgeLen;
        CurFlow = 0;
        flowInstance = flInstance;
        startPos = flowInstance.transform.position;
    }

    private void OnMouseDown()
    {
        swipeStart = Input.mousePosition;
        Debug.Log($"Mouse down {swipeStart}");
        isSwiping = true;
    }

    private void OnMouseUp()
    {
        if (!isSwiping) return;
        isSwiping = false;

        var mousePos = Input.mousePosition;
        float distance = Vector2.Distance(swipeStart, mousePos);
        Debug.Log($"mouse up {mousePos} {distance}");
        if (distance >= SwipeThreshold)
        {
            Debug.Log($"{(int)(distance / SwipeThreshold)}");
            IncreaseFlow((int)(distance / SwipeThreshold));
        }
    }

    private void IncreaseFlow(int multi)
    {
        CurFlow = Mathf.Clamp(CurFlow + multi, 0, Capacity);
        UpdateFlowVisual();
    }

    private void UpdateFlowVisual()
    {
        float normalized = CurFlow / (float)Capacity;

        Debug.Log($"{CurFlow} /{Capacity}");
        Vector3 scale = flowInstance.transform.localScale;
        scale.z = edgeLength * normalized;
        flowInstance.transform.localScale = scale;
        flowInstance.transform.position = startPos + flowInstance.transform.forward * 
                                            (edgeLength * normalized / 2f);
        // a                                            
    }

    public void IncreaseFlowAmount(int delta)
    {
        CurFlow = (CurFlow + delta) % Capacity;
    }
}