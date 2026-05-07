using Alg1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    LevelCreator creator;
    public int rows = 5;
    public int cols = 5;
    Bounds bounds;
    [SerializeField] GameObject segment;
    public EventHandler playerWon;
    public List<List<Vertex>> Graph { get; private set; }

    void Awake()
    {
        creator = new LevelCreator(rows, cols);
        
        Graph = creator.ReturnCompleteLevel();

        bounds = GetWindowSize();
    }
    private void Start()
    {
        var segmentInstance = segment.GetComponentInChildren<MatchSegmentWithLevel>();
        segmentInstance.ResizeFloor(bounds);

        Camera.main.GetComponent<CameraFitter>().FitCamera(segment.transform);
    }

    public Bounds GetWindowSize()
    {
        return creator.GetWindowSize();
    }
    public void CheckIfWin(object sender, EventArgs e)
    {
        if (sender.GetType() == typeof(VertexView1))
        {
            Vertex v = ((VertexView1)sender).vertex;
            HashSet<Vertex> visited = new HashSet<Vertex>();
            int res = CheckDfs(v, visited, cols * rows);
            if (res == 0)
            {
                Debug.Log("You've won!");
                playerWon?.Invoke(this, EventArgs.Empty);
                Camera.main.GetComponent<VictoryCamera>().OnPlayerWon();
            }
        }
    }

    private int CheckDfs(Vertex start, HashSet<Vertex> visited, int total)
    {
        Stack<Vertex> stack = new Stack<Vertex>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            Vertex current = stack.Pop();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            if (current.isRotateAble)
                total--;
            foreach (Edge e in current.Edges.Keys)
            {
                Vertex end = e.start == current ? e.end : e.start;
                if (!visited.Contains(end))
                {
                    stack.Push(end);
                }
            }
        }
        return total;
    }
}
