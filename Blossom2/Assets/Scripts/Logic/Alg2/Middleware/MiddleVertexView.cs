using UnityEngine;
using Alg2.Domains;
using TMPro;

public class MiddleVertexView : VertexViewParent
{
    [SerializeField] GameObject labelObj;
    [SerializeField] Transform valve;
    public float DegreesPerFlow = 50f;
    public float DegreesPerFrame = 1f;
    TMP_Text flowLabel;
    public int Capacity { get; private set; }

    float targetAngle = 0f;
    float rotatedAngle = 0f;

    

    public override void Init(Vertex v, int capacity)
    {
        vertex = v;
        ind = v.ind;
        Capacity = capacity;
        CurFlow = 0;

        flowLabel = labelObj.GetComponent<TMP_Text>();
        if (flowLabel != null)
            flowLabel.text = $"0/{Capacity}";
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

    public void OnFlowDecreased(int delta) => DecreaseFlow(delta);

    public override void IncreaseFlow(int delta)
    {
        CurFlow += delta;
        UpdateLabel();
        if (CurFlow > Capacity)
            Debug.Log($"Vertex {ind} overloaded: {CurFlow}/{Capacity}");
    }

    public override bool DecreaseFlow(int delta)
    {
        if (CurFlow >= delta)
        {
            CurFlow -= delta;
            UpdateLabel();
            return true;
        }
        return false;
    }

    private void UpdateLabel()
    {
        if (flowLabel != null)
            flowLabel.text = $"{CurFlow}/{Capacity}";
    }
}