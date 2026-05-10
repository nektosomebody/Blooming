using System;
using Alg2.Domains;
using UnityEngine;

public class TargetVertex : VertexViewParent
{
    private static readonly int BloomingHash = Animator.StringToHash("Blooming");

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

    public void PlayVictoryAnimation()
    {
        Debug.Log($"TargetVertex {ind} is blooming!");
        Animator animator = GetComponentInChildren<Animator>();
        animator.Play(BloomingHash);
    }
}