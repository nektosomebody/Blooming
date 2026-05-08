using System;
using System.Collections.Generic;
using System.Linq;

public class LevelResultManager
{
    public event Action OnWin;

    HashSet<TargetVertex> targetVertices = new();
    HashSet<MiddleVertexView> middleVertices = new();
    int _maxFlow;

    public int MaxFlow
    {
        get => _maxFlow;
        set
        {
            if (value > 0) _maxFlow = value;
            else throw new Exception("Max flow should be positive");
        }
    }

    public void Init(HashSet<TargetVertex> targets, HashSet<MiddleVertexView> middles, int maxFlow)
    {
        MaxFlow = maxFlow;
        targetVertices = targets;
        middleVertices = middles;

        foreach (var vert in targetVertices)
            vert.FlowChanged += CheckIfWin;
    }

    void CheckIfWin(object sender, EventArgs e)
    {
        int totalFlow = targetVertices.Sum(v => v.FlowAmount);
        if (totalFlow < MaxFlow) return;

        bool anyOverloaded = middleVertices.Any(m => m.CurFlow > m.Capacity);
        if (anyOverloaded) return;

        OnWin?.Invoke();
    }
}