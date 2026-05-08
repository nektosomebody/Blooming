using UnityEngine;
using Alg2.Domains;
using TMPro;

public class MiddleVertexView : VertexViewParent
{
    [SerializeField] GameObject labelPrefab;
    [SerializeField] float yUpper = 10f;
    [SerializeField] float zUpper = 10f;
    TMP_Text flowLabel;
    public int Capacity { get; private set; }

    public override void Init(Vertex v, int capacity)
    {
        vertex = v;
        ind = v.ind;
        Capacity = capacity;
        CurFlow = 0;

        if (labelPrefab != null)
        {
            GameObject labelObj = Instantiate(labelPrefab,
                transform.position + new Vector3(0f, yUpper, zUpper),
                Quaternion.Euler(90f, 0f, 0f));
            flowLabel = labelObj.GetComponent<TMP_Text>();
            if (flowLabel != null)
                flowLabel.text = $"0/{Capacity}";
        }
    }

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