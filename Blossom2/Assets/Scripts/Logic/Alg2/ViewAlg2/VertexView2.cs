using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alg2.Domains;

public class VertexView2 : MonoBehaviour
{
    [SerializeField] int ind;
    Vertex vertex;
    public int FlowAmount { get; private set; } = 0;

    public void Init(Vertex v)
    {
        vertex = v;
        ind = vertex.ind;
    }
    public void IncreaseFlow(int delta)
    {
        FlowAmount += delta;
    }
    
}
