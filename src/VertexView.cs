using LevelGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VertexView : MonoBehaviour
{
    public GameObject edgePrefab;
    public Vertex vertex;
    public List<EdgeView> edgesViews = new List<EdgeView>();

    public void Start()
    {
        transform.position = new Vector3(vertex.X, vertex.Y, 0);
    }

    public void Init(Vertex v)
    {
        vertex = v;
        CreateEdges();
    }
    public void CreateEdges()
    {
        for (int i = 0; i < vertex.edges.Count; i++) 
        {
            if (vertex.isActive[vertex.edges[i]] == true)
            {
                EdgeView tmp = new EdgeView();
                tmp.Init(vertex.edges[i]);
                edgesViews.Add(tmp);
            }
        }
    }

    private void OnMouseDown()
    {
        vertex.ClickAction();
    }
}
