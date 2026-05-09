using UnityEngine;
using Alg2.Domains;

public class SourceVertexView : VertexViewParent
{
    public override void Init(Vertex v, int capacity)
    {
        vertex = v;
        ind = v.ind;
    }

    public override void IncreaseFlow(int delta) { }

    public override void DecreaseFlow(int delta) { }
}