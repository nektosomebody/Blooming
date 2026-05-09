using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class LevelHolder2 : LevelData
{
    [SerializeField] TMP_Text overloadLabel;
    // [SerializeField] string overloadTableName = "MainMenu";
    [SerializeField] LocalizedString overloadString;
    private bool _hasWon = false;
    int _bloomedCount;
    HashSet<SourceVertexView> sourceVertices = new();
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

    public void Init(HashSet<TargetVertex> targets, HashSet<MiddleVertexView> middles, int maxFlow, HashSet<SourceVertexView> sources)
    {
        MaxFlow = maxFlow;
        targetVertices = targets;
        middleVertices = middles;
        sourceVertices = sources;

        _hasWon = false;
        _bloomedCount = 0;
        foreach (var vert in targetVertices)
        {
            vert.FlowChanged += CheckIfWin;
            vert.BloomingFinished += OnTargetBloomed;
        }
    }

    public void PlayTargetVictoryAnimations()
    {
        foreach (var targetVertex in targetVertices)
        {
            targetVertex.PlayVictoryAnimation();
        }
    }

    public Vector3 GetLevelCenter()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var middle in middleVertices)
        {
            sum += middle.transform.position;
            count++;
        }
        foreach (var middle in targetVertices)
        {
            sum += middle.transform.position;
            count++;
        }

        if (count == 0)
            return Vector3.zero;

        return sum / count;
    }

    public void OnVertexOverloadChanged(object sender, OverloadChangedArgs e)
    {
        if (overloadLabel == null) return;
        if (e.IsOverloaded)
        {
            string template = overloadString.GetLocalizedString();
            overloadLabel.text = string.Format(template, e.Capacity);
            overloadLabel.gameObject.SetActive(true);
        }
        else
        {
            overloadLabel.gameObject.SetActive(false);
        }
    }

    void CheckIfWin(object sender, EventArgs e)
    {
        if (_hasWon) return;
        int totalFlow = targetVertices.Sum(v => v.FlowAmount);
        Debug.Log($"Total flow: {totalFlow}, Max flow: {MaxFlow}");

        if (totalFlow < MaxFlow) return;
        bool anyOverloaded = middleVertices.Any(m => m.CurFlow > m.Capacity);
        if (anyOverloaded) return;

        _hasWon = true;
        foreach (var source in sourceVertices)
            source.TriggerAnimations();
    }

    void OnTargetBloomed()
    {
        _bloomedCount++;
        if (_bloomedCount == targetVertices.Count)
            RaisePlayerWon();
    }
}