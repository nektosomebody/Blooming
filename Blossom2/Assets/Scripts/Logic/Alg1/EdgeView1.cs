using Alg1;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class EdgeView1 : MonoBehaviour
{
    [SerializeField] public float speed = 1f;

    Edge edge;
    bool shrinkEdge = false;
    bool increaseEdge = false;
    float t = 1f;
    Vector3 start;
    Vector3 end;
    float fullLength;

    private void Update()
    {
        if (shrinkEdge)
        {
            t -= speed * Time.deltaTime;
            UpdateEdge();

            if (t <= 0f)
            { 
                shrinkEdge = false;
                increaseEdge = true;
                UpdateVertexes();
            }
        }

        else if (increaseEdge)
        {
            t += speed * Time.deltaTime;
            t = Mathf.Clamp01(t);

            UpdateEdge();

            if (t >= 1f)
            {
                t = 1f;
                increaseEdge = false;
            }
        }
    }

    void UpdateEdge()
    {
        Vector3 dir = (end - start).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        float currentLength = fullLength * t;
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            currentLength / 2f
        );

        transform.position = start + dir * (currentLength / 2f);
    }

    void UpdateVertexes()
    {
        start = new Vector3(edge.start.X, 0, edge.start.Y);
        end = new Vector3(edge.end.X, 0, edge.end.Y);

    }

    public void Init(Edge e)
    {
        edge = e;
        edge.start.ClickedEvent += RotateEdge;

        UpdateVertexes();
        fullLength = Vector3.Distance(start, end);
        
        UpdateEdge();
    }

    private void RotateEdge(object sender, EventArgs e)
    {
        shrinkEdge = true;     
        return;
    }

    private void OnDestroy()
    {
        edge.start.ClickedEvent -= RotateEdge;
    }
}