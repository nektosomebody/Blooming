using Alg1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VertexView1 : MonoBehaviour
{
    public GameObject edgePrefab;
    public Vertex vertex;
    public EventHandler ClickedEvent;
    [SerializeField] GameObject playerAnim;
    bool isRotating;

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up * 100f * Time.deltaTime);
        }
    }
    public void UpdatePos()
    {
        transform.position = new Vector3(vertex.X, 0, vertex.Y);
        
    }

    public void Init(Vertex v)
    {
        vertex = v;
        UpdatePos();
        CreateEdges();
    }
    public void CreateEdges()
    {
        if (!vertex.isRotateAble) { return; } // only rotateAble vertexes know about their edges
        foreach (Edge edge in vertex.Edges.Keys)
        {
            GameObject edge_obj = Instantiate(edgePrefab);
            EdgeView1 tmp = edge_obj.GetComponent<EdgeView1>();
            tmp.Init(edge);
        }
    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked!!" + vertex.ind.ToString());
        vertex.ClickAction();
        ClickedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void Rotate(object sender, EventArgs e)
    {
        isRotating = true;
    }
    public void PlayBloomingAnimation(object sender, EventArgs e)
    {
        GetComponentInChildren<Animator>().Play("Blooming");
    }
}
