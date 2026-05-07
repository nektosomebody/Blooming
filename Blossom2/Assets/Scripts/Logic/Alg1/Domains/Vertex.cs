using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Alg1
{
    public class Vertex
    {
        public int ind;
        public bool isPartOfPath;
        public bool isRotateAble;
        public float X { get; private set; }
        public float Y { get; private set; }
        public HashSet<Vertex> AllNeighbors { get; private set; }
        public List<Vertex> AllSortedNeighbours { get; private set; }
        public Dictionary<Edge, int> Edges { get; private set; }

        public EventHandler ClickedEvent;

        public Vertex(int i, float x, float y, bool isPart=false, bool isRotateAble=false)
        {
            ind = i;
            X = x;
            Y = y;
            isPartOfPath = isPart;
            AllNeighbors = new HashSet<Vertex>();
            AllSortedNeighbours = new List<Vertex>();
            Edges = new();
            this.isRotateAble = isRotateAble;
        }

        public void AddEdge(Edge edg)
        {
            if (AllSortedNeighbours.Count != AllNeighbors.Count)
            {
                SortClockwise();
            }
            Edges[edg] = AllSortedNeighbours.IndexOf(edg.end);

        }

        public void AddNeighbour(Vertex v)
        {
            AllNeighbors.Add(v);
        }

        public void SortClockwise()
        {
            AllSortedNeighbours = AllNeighbors
                .OrderByDescending(p =>
                {
                    var angle = Math.Atan2(p.Y - Y, p.X - X);
                    return (angle + 2 * Math.PI) % (2 * Math.PI);
                })
                .ToList();
        }
        private void RemoveEdge(Edge edg)
        {
            Edges.Remove(edg);
        }
        public void ClickAction()
        {
            if (!isRotateAble) return;
            if (Edges.Count == 0) return;
            foreach (Edge edge in Edges.Keys.ToList())
            {
                edge.end.RemoveEdge(edge);
                Edges[edge] = (Edges[edge] + 1) % AllNeighbors.Count;
                edge.Rotate(AllSortedNeighbours[Edges[edge]]);
                edge.end.AddEdge(edge);
            }
            ClickedEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}
