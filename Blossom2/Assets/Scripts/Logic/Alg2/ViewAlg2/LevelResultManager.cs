using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelResultManager
{
    private HashSet<TargetVertex> targetVertices = new();
    int _maxFlow;
    public int MaxFlow
    {
        get => _maxFlow;
        set
        {
            if (value > 0)
            {
                _maxFlow = value;
                Debug.Log($"Max flow set to {value}");
            }
            else 
            {
                throw new System.Exception("Max flow should be positive");
            }
        }
    }
    public void CheckIfWin(object sender, System.EventArgs eventArgs)
    {
        int summ = targetVertices.Sum(v => v.FlowAmount);
        if (summ >= MaxFlow)
        {
            Debug.LogWarning("Win");
        }
    }

    public void Init(HashSet<TargetVertex> verts, int maxFlow)
    {
        MaxFlow = maxFlow;
        targetVertices = verts;
        
        foreach (var vert in targetVertices)
        {
            vert.FlowChanged += CheckIfWin;
        }
    }

}