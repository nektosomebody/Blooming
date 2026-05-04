using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelGenerator
{
    public class Vertex
    {
        readonly int ind = 0;
        public float X { get; set; }
        public float Y { get; set; }
        public HashSet<Vertex> all_neighbors = new HashSet<Vertex>();
        public List<Vertex> all_sorted_neighbours = new List<Vertex>();
        public List<Edge> edges = new List<Edge>();
        public Dictionary<Edge, bool> isActive = new Dictionary<Edge, bool>();


        public Vertex(int ind, float x, float y)
        {
            this.ind = ind;
            this.X = x;
            this.Y = y;
        }

        public void AddEdge(Edge edge, bool active)
        {
            if (edges.Contains(edge))
            {
                if (isActive[edge] != active)
                {
                    throw new Exception("AAA");
                }
            }
            edges.Add(edge);
            isActive[edge] = active;
        }
        // если повернули соседнюю вершину
        // и больше не имеем доступа к этому ребру
        public void DeleteEdge(Edge edge)
        {
            isActive.Remove(edge);
            edges.Remove(edge);

        }

        public void SortClockwise()
        {
            all_sorted_neighbours = all_neighbors
                .OrderByDescending(p => Math.Atan2(p.Y - Y, p.X - X))
                .ToList();
        }

        public void ClickAction()
        {
            if (edges.Count == 0) return;
            for (int i = 0; i < edges.Count; ++i)
            {
                if (isActive[edges[i]])
                {
                    edges[i].Rotate(this);
                }
            }
        }

        public void AddToAllNeighbours(Vertex neighbour)
        {
            all_neighbors.Add(neighbour);
        }
    }
}
