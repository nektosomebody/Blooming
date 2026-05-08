using System;
using Alg2.Domains;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class EdgeWithFlowView : MonoBehaviour
{
    [SerializeField] GameObject labelPrefab;
    TMP_Text flowLabel;
    // need to make tmp upper than tube
    public float y_upper = 10f;
    public float z_upper = 10f;
    EdgeWithFlow edge;
    public event Action<int> FlowChanged;
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

        if (labelPrefab != null)
        {
            GameObject labelObj = Instantiate(labelPrefab, transform.position + new Vector3(0f, y_upper, z_upper), Quaternion.Euler(90f, 0f, 0f));
            flowLabel = labelObj.GetComponent<TMP_Text>();
            if (flowLabel != null)
                flowLabel.text = $"0/{Capacity}";
        }
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
        int oldFlow = CurFlow;
        CurFlow = Mathf.Clamp(CurFlow + multi, 0, Capacity);
        int delta = CurFlow - oldFlow;
        if (delta > 0)
        {
            UpdateFlowVisual();
            FlowChanged?.Invoke(delta);
        }
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
        if (flowLabel != null)
            flowLabel.text = $"{CurFlow}/{Capacity}";
    }

    public void ResetFlow(object sender, EventArgs e)
    {
        edge.ResetFlow();
    }
    public void IncreaseFlowAmount(int delta)
    {
        CurFlow = (CurFlow + delta) % Capacity;
    }
}