using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelResultManager : LevelData
{
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
        Debug.Log($"Total flow: {totalFlow}, Max flow: {MaxFlow}");
        
        if (totalFlow < MaxFlow) return;
        bool anyOverloaded = middleVertices.Any(m => m.CurFlow > m.Capacity);
        if (anyOverloaded) return;

        RaisePlayerWon();
    }
}