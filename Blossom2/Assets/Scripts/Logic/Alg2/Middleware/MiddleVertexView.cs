using UnityEngine;
using Alg2.Domains;
using TMPro;
using System;
using System.Collections.Generic;

public class OverloadChangedArgs : EventArgs
{
    public int CurFlow { get; }
    public int Capacity { get; }
    public bool IsOverloaded { get; }
    public OverloadChangedArgs(int curFlow, int capacity, bool isOverloaded)
    { CurFlow = curFlow; Capacity = capacity; IsOverloaded = isOverloaded; }
}

public class MiddleVertexView : VertexViewParent
{
    [SerializeField] GameObject labelObj;
    [SerializeField] Transform valve;
    public float DegreesPerFlow = 50f;
    public float DegreesPerFrame = 1f;
    TMP_Text flowLabel;
    public int Capacity { get; private set; }
    public event EventHandler MidFlowDecreased;
    public event EventHandler<OverloadChangedArgs> OverloadStateChanged;
    private bool _wasOverloaded = false;
    private int _incomingEdgeCount = 0;
    private int _arrivedCount = 0;
    private Color originalColor;
    private float originalFontSize;
    private const float OverloadedFontSize = 140f;
    private static readonly Color OverloadedColor = new Color(1, 0, 0, 1);

    float targetAngle = 0f;
    float rotatedAngle = 0f;

    private int totalIncomingFlow = 0;
    private int totalOutgoingFlow = 0;
    private List<EdgeWithFlowView> outgoingEdges = new();

    public override void Init(Vertex v, int capacity)
    {
        vertex = v;
        ind = v.ind;
        Capacity = capacity;
        CurFlow = 0;

        flowLabel = labelObj.GetComponent<TMP_Text>();
        if (flowLabel != null)
        {
            flowLabel.text = $"0/{Capacity}";
            originalColor = flowLabel.color;
            originalFontSize = flowLabel.fontSize;
        }
    }

    void Update()
    {
        if (valve == null || Mathf.Approximately(rotatedAngle, targetAngle)) return;
        float diff = targetAngle - rotatedAngle;
        float step = Mathf.Min(DegreesPerFrame, Mathf.Abs(diff));
        if (diff > 0)
        {
            rotatedAngle += step;
            valve.Rotate(Vector3.forward, -step);
        }
        else
        {
            rotatedAngle -= step;
            valve.Rotate(Vector3.forward, step);
        }
    }

    public void StartRotation(int delta)
    {
        targetAngle += delta * DegreesPerFlow;
    }

    public void ReverseRotation(int delta)
    {
        targetAngle = Mathf.Max(0f, targetAngle - delta * DegreesPerFlow);
    }

    public void RegisterOutgoingEdge(EdgeWithFlowView edge)
    {
        outgoingEdges.Add(edge);
    }

    public void RegisterIncomingEdge() => _incomingEdgeCount++;

    public override void OnFlowArrived()
    {
        _arrivedCount++;
        if (_arrivedCount == 1)
        {
            foreach (var edge in outgoingEdges)
                edge.PlayFlowAnimation();
        }
    }

    public void OnIncomingFlowChanged(int oldFlow, int newFlow)
    {
        totalIncomingFlow += (newFlow - oldFlow);
        Debug.Log($"Vertex {ind}: incoming flow changed from {oldFlow} to {newFlow}, total: {totalIncomingFlow}");
        EnforceFlowConservation();
    }

    private void EnforceFlowConservation()
    {
        int excess = totalOutgoingFlow - totalIncomingFlow;
        Debug.Log($"Vertex {ind}: enforcing conservation, excess: {excess}");
        CascadeDecreaseOutgoing(excess);
    }

    private void CascadeDecreaseOutgoing(int excess)
    {
        if (outgoingEdges.Count == 0) return;

        int stillNeedToDecrease = excess;

        foreach (var edge in outgoingEdges)
        {
            if (stillNeedToDecrease <= 0) break;

            int canDecreaseFromThisEdge = edge.CurFlow;
            int decreaseAmount = Mathf.Min(stillNeedToDecrease, canDecreaseFromThisEdge);

            if (decreaseAmount > 0)
            {
                edge.ResetCurFlow(null, EventArgs.Empty);
                stillNeedToDecrease -= decreaseAmount;
                totalOutgoingFlow -= decreaseAmount;
            }
        }

        Debug.Log($"Vertex {ind}: cascaded {excess - stillNeedToDecrease} units, remaining: {stillNeedToDecrease}");
    }

    public void OnOutgoingFlowChanged(int oldFlow, int newFlow)
    {
        totalOutgoingFlow += (newFlow - oldFlow);
        Debug.Log($"Vertex {ind}: outgoing flow changed, total: {totalOutgoingFlow}");

    }

    public override void IncreaseFlow(int delta)
    {
        CurFlow += delta;
        UpdateLabel();
    }

    public override void DecreaseFlow(int delta)
    {
        if (CurFlow >= delta)
        {
            CurFlow -= delta;
        }
        else
        {
            CurFlow = 0;
        }
        UpdateLabel();
        MidFlowDecreased?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateLabel()
    {
        if (flowLabel != null)
        {
            flowLabel.text = $"{CurFlow}/{Capacity}";
            bool isOverloaded = VertexIsOverloaded();
            if (isOverloaded)
            {
                flowLabel.color = OverloadedColor;
                flowLabel.fontSize = OverloadedFontSize;
                Debug.Log($"Vertex {ind} overloaded: {CurFlow}/{Capacity}");
            }
            else
            {
                flowLabel.color = originalColor;
                flowLabel.fontSize = originalFontSize;
            }

            if (isOverloaded != _wasOverloaded)
            {
                _wasOverloaded = isOverloaded;
                OverloadStateChanged?.Invoke(this, new OverloadChangedArgs(CurFlow, Capacity, isOverloaded));
            }
        }
    }
    public bool VertexIsOverloaded()
    {
        Debug.Log($"Vertex {ind}: total incoming flow = {totalIncomingFlow}, total outgoing flow = {totalOutgoingFlow}, capacity = {Capacity}, curFlow = {CurFlow}");
        return totalIncomingFlow > Capacity;
    }
}