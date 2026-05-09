using UnityEngine;
using Alg2.Domains;

public abstract class VertexViewParent : MonoBehaviour
{
    [SerializeField] protected int ind;

    protected Vertex vertex;
    public int CurFlow { get; protected set; }
    public int Ind => ind;

    public abstract void Init(Vertex v, int capacity);
    public abstract void IncreaseFlow(int delta);
    public abstract void DecreaseFlow(int delta);
}