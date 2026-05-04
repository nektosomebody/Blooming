using DelaunatorSharp;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace LevelGenerator
{
    public class LevelCreator
    {
        public List<Vertex> Create(int rows, int cols)
        {
            IPoint[] points = new IPoint[rows * cols];

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    points[3 * i + j] = new Point(i * 10, j * 10);
                }
            }
            Delaunator delaunator = new Delaunator(points);
            List<Vertex> graph = Enumerable.Repeat<Vertex>(null, rows * cols).ToList();
            for (int i = 0; i < delaunator.Triangles.Length; i += 3)
            {
                for (int j = 0; j < 3; ++j)
                {
                    int triIndex = delaunator.Triangles[i + j];

                    if (graph[triIndex] == null)
                    {
                        IPoint coords = points[triIndex];
                        graph[triIndex] = new Vertex(triIndex, (float)coords.X, (float)coords.Y);
                    }
                }
                int tIndx1 = delaunator.Triangles[i];
                int tIndx2 = delaunator.Triangles[i + 1];
                int tIndx3 = delaunator.Triangles[i + 2];
                graph[tIndx1].AddToAllNeighbours(graph[tIndx2]);
                graph[tIndx2].AddToAllNeighbours(graph[tIndx1]);
                graph[tIndx1].AddToAllNeighbours(graph[tIndx3]);
                graph[tIndx3].AddToAllNeighbours(graph[tIndx1]);
                graph[tIndx2].AddToAllNeighbours(graph[tIndx3]);
                graph[tIndx3].AddToAllNeighbours(graph[tIndx2]);
            }
            for (int i = 0; i < graph.Count; ++i)
            {
                graph[i].SortClockwise();
            }

            List<Vertex> q = BuildMinimalOstov(graph[0], rows * cols);
            for (int i = 1; i < q.Count; ++i)
            {
                Vertex start = q[i - 1];
                Vertex end = q[i];

                Edge tmp = new Edge(start, end);

                // поворачивать можно только одну вершину,
                // поэтому для второй ребро будет недоступно
                start.AddEdge(tmp, true);
                end.AddEdge(tmp, false);
            }
            return graph;
        }


        private List<Vertex> BuildMinimalOstov(Vertex start, int total)
        {
            var path = new List<Vertex>();
            var visited = new HashSet<Vertex>();
            Dfs(start, visited, path, total);
            return path;
        }

        private void Dfs(Vertex v, HashSet<Vertex> visited, List<Vertex> path, int total)
        {
            visited.Add(v);
            path.Add(v);

            foreach (var n in v.all_neighbors)
            {
                if (!visited.Contains(n))
                {
                    Dfs(n, visited, path, total);
                    // path.Add(v);
                }
                if (visited.Count == total)
                {
                    return;

                }
            }

        }

    }
}