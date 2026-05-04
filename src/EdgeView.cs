using LevelGenerator;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EdgeView : MonoBehaviour
{
    public LineRenderer line;
    Edge edge;

    private void Update()
    {
        line.positionCount = 2;
        line.SetPosition(0, edge.GetLeft());
        line.SetPosition(1, edge.GetRigth());
    }
    public void Init(Edge e)
    {
        edge = e;
    }
    

    
}


/*
 // индекс в массиве правых вершин
    int cur_left;
    // аналогично в левых
    int cur_right;
    // все соседи левой вершины
    public List<VertexView> lefts;
    // аналогично с правой
    public List<VertexView> rigths;

    public void Init(VertexView left, VertexView rigth)
    {
        lefts = left.all_sorted_neighbours;
        for (int i = 0; i < lefts.Count; ++i)
        {
            if (lefts[i] == rigth) { cur_right = i; break; }
        }
        rigths = rigth.all_sorted_neighbours;
        for (int i = 0; i < rigths.Count; ++i)
        {
            if (rigths[i] == left) { cur_left = i; break; }
        }
    }
    public void SetPoints(Vector3 a, Vector3 b)
    {
        line.positionCount = 2;
        line.SetPosition(0, a);
        line.SetPosition(1, b);
    }

    public void Rotate()
    {
        Vector3 p0 = line.GetPosition(0);
        Vector3 p1 = line.GetPosition(1);
        Vector3 dir = p1 - p0;
        Vector3 rotatedDir = new Vector3(-dir.y, dir.x, 0);

        line.SetPosition(1, p0 + rotatedDir);
    }


    /// <param name="center">вершина, вокруг которой вращаем</param>
    public void Rotate(VertexView center)
    {
        if (center == lefts[cur_right])
        {
            RotateAroundRigth();
        }
        else
        {
            RotateAroundLeft();
        }
    }

    private void RotateAroundRigth()
    {
        rigths[cur_left].DeleteEdge(this);
        cur_left = (cur_left + 1) % rigths.Count;
        rigths[cur_left].AddEdge(this, false);
    }

    private void RotateAroundLeft()
    {
        lefts[cur_right].DeleteEdge(this);
        cur_right = (cur_right + 1) % lefts.Count;
        lefts[cur_right].AddEdge(this, false);
    }
 */