using Alg1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] GameObject segment;
    [SerializeField] int initialSize = 2;
    [SerializeField] int delta = 3;

    LevelCreator creator;
    public int rows;
    public int cols;
    Bounds bounds;
    TimerUI timer;
    public EventHandler playerWon;
    public List<List<Vertex>> Graph { get; private set; }
    const string LevelsKey = "alg1_levels_completed";

    void Awake()
    {
        int completed = PlayerPrefs.GetInt(LevelsKey, 0);
        rows = cols = ComputeSize(completed);
        creator = new LevelCreator(rows, cols);
        Graph = creator.ReturnCompleteLevel();

        bounds = GetWindowSize();
    }

    int ComputeSize(int completed)
    {
        int size = initialSize;
        int threshold = delta;
        while (completed >= threshold)
        {
            size++;
            completed -= threshold;
            threshold *= 2;
        }
        return size;
    }
    private void Start()
    {
        timer = GetComponent<TimerUI>();
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
                PlayerPrefs.SetInt(LevelsKey, PlayerPrefs.GetInt(LevelsKey, 0) + 1);
                PlayerPrefs.Save();
                timer?.StopTimer();
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
