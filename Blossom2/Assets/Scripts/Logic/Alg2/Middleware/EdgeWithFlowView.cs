using System;
using System.Collections;
using Alg2.Domains;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class EdgeWithFlowView : MonoBehaviour
{
    [SerializeField] GameObject labelPrefab;
    [SerializeField] float SwipeThreshold = 30f;
    TMP_Text flowLabel;
    // need to make tmp upper than tube and flow
    public Vector3 flowSpacing = new Vector3(0f, 10f, 0f);
    public Vector3 labelSpacing = new Vector3(0f, 20f, 10f);
    EdgeWithFlow edge;
    MiddleVertexView startMiddleVertex;
    MiddleVertexView targetMiddleVertex;
    public event Action<int> FlowIncreased;
    public event Action<int> FlowDecreased;
    public int Capacity { get; private set; }
    public int CurFlow { get; private set; }
    float _flowAnimSpeed = 3f;
    Action _onFlowAnimComplete;
    float edgeLength;
    Vector2 swipeStart;
    bool isSwiping;
    GameObject flowInstance;
    Vector3 startPos;
    Vector3 posFrom;
    Vector3 posTo;

    

    public void Init(EdgeWithFlow e, float edgeLen, GameObject flInstance, VertexViewParent startVertex, Vector3 from, Vector3 to)
    {
        startMiddleVertex = startVertex as MiddleVertexView;
        edge = e;
        Capacity = edge.Cap;
        edgeLength = edgeLen;
        CurFlow = 0;
        flowInstance = flInstance;
        startPos = flowInstance.transform.position + flowSpacing;
        posFrom = from;
        posTo = to;
        _flowAnimSpeed = Capacity;

        GameObject labelObj = Instantiate(labelPrefab, transform.position + labelSpacing, Quaternion.Euler(90f, 0f, 0f));
        flowLabel = labelObj.GetComponent<TMP_Text>();
        if (flowLabel != null)
            flowLabel.text = $"0/{Capacity}";

    }

    public void SetTargetVertex(VertexViewParent target)
    {
        targetMiddleVertex = target as MiddleVertexView;
    }

    public void SetAnimationCallback(Action onComplete)
    {
        _onFlowAnimComplete = onComplete;
    }

    public void PlayFlowAnimation()
    {
        StartCoroutine(AnimateFlow());
    }

    private IEnumerator AnimateFlow()
    {
        float speed = Capacity * _flowAnimSpeed;
        float currentLength = 0f;
        Vector3 scale = flowInstance.transform.localScale;
        scale.z = 0f;
        flowInstance.transform.localScale = scale;
        while (currentLength < edgeLength)
        {
            currentLength = Mathf.Min(currentLength + speed * Time.deltaTime, edgeLength);
            scale.z = currentLength;
            flowInstance.transform.localScale = scale;
            flowInstance.transform.position = startPos + flowInstance.transform.forward * (currentLength / 2f);
            yield return null;
        }
        _onFlowAnimComplete?.Invoke();
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

        Vector2 screenFrom = Camera.main.WorldToScreenPoint(posFrom);
        Vector2 screenTo = Camera.main.WorldToScreenPoint(posTo);
        Vector2 edgeDir = (screenTo - screenFrom).normalized;

        Vector2 swipeVec = (Vector2)Input.mousePosition - swipeStart;
        float projection = Vector2.Dot(swipeVec, edgeDir);

        int units = (int)(Mathf.Abs(projection) / SwipeThreshold);
        if (units == 0) return;

        if (projection > 0)
        {
            Debug.Log(projection);
            IncreaseFlow(units);
        }
        else
        {
            Debug.Log(projection);
            DecreaseFlow(units);
        }
    }

    private void IncreaseFlow(int multi)
    {
        int availableFlow = startMiddleVertex != null ? startMiddleVertex.CurFlow : int.MaxValue;
        int canAdd = Mathf.Min(multi, availableFlow);
        int oldFlow = CurFlow;
        CurFlow = Mathf.Clamp(CurFlow + canAdd, 0, Capacity);
        int delta = CurFlow - oldFlow;
        if (delta <= 0) return;

        if (startMiddleVertex != null && startMiddleVertex.VertexIsOverloaded())
        {
            Debug.Log("Cannot increase flow, vertex is overloaded");
            CurFlow = oldFlow;
            return;
        }
        startMiddleVertex?.OnOutgoingFlowChanged(oldFlow, CurFlow);
        targetMiddleVertex?.OnIncomingFlowChanged(oldFlow, CurFlow);
        startMiddleVertex?.DecreaseFlow(delta);
        FlowIncreased?.Invoke(delta);
        UpdateFlowLabel();
        if (startMiddleVertex != null) startMiddleVertex.StartRotation(delta);
    }

    private void DecreaseFlow(int multi)
    {
        int oldFlow = CurFlow;
        CurFlow = Mathf.Clamp(CurFlow - multi, 0, Capacity);
        int delta = oldFlow - CurFlow;
        if (delta > 0)
        {
            Debug.Log($"start: {startMiddleVertex?.Ind} end {targetMiddleVertex?.Ind} decrease flow by {delta}");
            startMiddleVertex?.OnOutgoingFlowChanged(oldFlow, CurFlow);
            targetMiddleVertex?.OnIncomingFlowChanged(oldFlow, CurFlow);
            startMiddleVertex?.IncreaseFlow(delta);
            FlowDecreased?.Invoke(delta); // targetMiddleVertex?.DecreaseFlow(delta);
            
            // UpdateFlowVisual();
            UpdateFlowLabel();
            
            if (startMiddleVertex != null) startMiddleVertex.ReverseRotation(delta);
        }
    }

    private void UpdateFlowInstance()
    {
        float normalized = CurFlow / (float)Capacity;

        Debug.Log($"{CurFlow} /{Capacity}");
        Vector3 scale = flowInstance.transform.localScale;
        scale.z = edgeLength * normalized;
        flowInstance.transform.localScale = scale;
        flowInstance.transform.position = startPos + flowInstance.transform.forward *
                                            (edgeLength * normalized / 2f);
    }
    private void UpdateFlowLabel()
    {
       if (flowLabel != null)
            flowLabel.text = $"{CurFlow}/{Capacity}";
    }
    public void ResetCurFlow(object sender, EventArgs e)
    {
        DecreaseFlow(CurFlow);
    }

    public void ResetFlow(object sender, EventArgs e)
    {
        edge.ResetFlow();
    }
}