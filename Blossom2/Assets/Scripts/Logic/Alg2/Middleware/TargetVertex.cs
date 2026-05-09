using System;
using Alg2.Domains;
using UnityEngine;

public class TargetVertex : VertexViewParent
{
    public int FlowAmount => CurFlow;
    public EventHandler FlowChanged;

    public override void Init(Vertex v, int capacity)
    {
        vertex = v;
        ind = v.ind;
    }

    public override void IncreaseFlow(int delta)
    {
        CurFlow += delta;
        FlowChanged?.Invoke(this, EventArgs.Empty);
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
    }
}