using UnityEngine;
using Alg2.Domains;
using System.Collections.Generic;

public class SourceVertexView : VertexViewParent
{
    List<EdgeWithFlowView> _animEdges = new();

    public override void Init(Vertex v, int capacity)
    {
        vertex = v;
        ind = v.ind;
    }

    public override void IncreaseFlow(int delta) { }

    public override void DecreaseFlow(int delta) { }

    public void RegisterOutgoingEdgeForAnimation(EdgeWithFlowView edge)
    {
        _animEdges.Add(edge);
    }

    public void TriggerAnimations()
    {
        foreach (var edge in _animEdges)
            edge.PlayFlowAnimation();
    }
}