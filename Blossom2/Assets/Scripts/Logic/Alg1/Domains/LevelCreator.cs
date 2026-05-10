using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Alg1
{
    public class LevelCreator
    {
        int rows_c;
        int cols_c;
        float delta;
        public List<List<Vertex>> graph { get; private set; } = new List<List<Vertex>>();

        public LevelCreator(int rows_c, int cols_c, float d=10)
        {
            this.rows_c = rows_c;
            this.cols_c = cols_c;
            delta = d;
        }

        public Bounds GetWindowSize()
        {
            Vector3 center = new(
                                (graph[1][0].X + graph[rows_c * 2 - 1][cols_c * 2].X) / 2,
                                0,
                                (graph[0][0].Y + graph[rows_c * 2][cols_c - 1].Y) / 2
                                );
            Vector3 size = new(
                                Math.Abs(graph[1][0].X - graph[rows_c * 2 - 1][cols_c * 2].X), 
                                0, 
                                Math.Abs(graph[0][0].Y - graph[rows_c * 2][cols_c - 1].Y)
                                );
            return new(center, size);
        }
        public List<List<Vertex>> ReturnCompleteLevel()
        {
            CreateLevel();
            int r, c;
            for (int i = 0; i < RandomNumberGenerator.GetInt32(rows_c * cols_c / 2, rows_c * cols_c + 1); i++)
            {
                Tuple<int, int> indexes = GetRandomRotateVertexIndexes();
                r = indexes.Item1;
                c = indexes.Item2;
                graph[r][c].ClickAction();
                Debug.Log($"{r} {c}");
            }
            return graph;
        }

        private void CreateLevel()
        {
            List<Vertex> line = new();
            int global_ind = 0;
            float center_x = 0;
            float center_y = 0;

            for (int i = 0; i < cols_c * 2; i += 2)
            {
                Vertex tmp_v = new(global_ind, center_x + delta + delta * i, center_y);
                line.Add(tmp_v);
                global_ind++;
            }
            graph.Add(line);


            for (int i = 1; i < rows_c * 2 + 1; i++)
            {
                line = new List<Vertex>();
                if (i % 2 == 1)
                {
                    Vertex tmp_v = new(global_ind, center_x, center_y + delta * i);
                    line.Add(tmp_v);
                    global_ind++;

                    for (int j = 1; j < cols_c * 2 + 1; j += 2)
                    {
                        Vertex tmp1 = new Vertex(global_ind, center_x + delta * j, center_y + delta * i, isPart: true, isRotateAble: true);

                        Vertex tmp2 = new Vertex(global_ind + 1, center_x + delta + delta * j, center_y + delta * i, isPart: true);
                        if (j == cols_c * 2 - 1)
                        {
                            tmp2.isPartOfPath = false;
                        }

                        tmp1.AddNeighbour(graph[i - 1][(j - 1) / 2]);
                        tmp1.AddNeighbour(line[j - 1]);
                        tmp1.AddNeighbour(tmp2);
                        if (graph[i - 1][(j - 1) / 2].isPartOfPath)
                        {
                            graph[i - 1][(j - 1) / 2].AddNeighbour(tmp1);
                        }
                        if (tmp2.isPartOfPath)
                        {
                            tmp2.AddNeighbour(tmp1);
                        }
                        if (line[j - 1].isPartOfPath)
                        {
                            line[j - 1].AddNeighbour(tmp1);
                        }

                        line.Add(tmp1);
                        line.Add(tmp2);
                        global_ind += 2;
                    }
                }
                else
                {
                    bool flag = true;
                    if (i == rows_c * 2)
                    {
                        flag = false;
                    }
                    for (int j = 0; j < cols_c * 2; j += 2)
                    {
                        Vertex v = new(global_ind, center_x + delta + delta * j, center_y + delta * i, isPart: flag);
                        graph[i - 1][j + 1].AddNeighbour(v);
                        if (flag)
                        {
                            v.AddNeighbour(graph[i - 1][j + 1]);
                        }
                        line.Add(v);
                        global_ind++;
                    }
                }
                graph.Add(line);
            }

            Tuple<int, int> indexes = GetRandomRotateVertexIndexes();
            int r = indexes.Item1;
            int c = indexes.Item2;
            var path = BuildMinOstov(graph[r][c], 3 * rows_c * cols_c - rows_c - cols_c);
            // var path = BuildMinOstov(graph[r][c], rows_c * cols_c);
            foreach (Vertex end in path.Keys)
            {
                Vertex start = path[end];
                Edge edge = new(start, end);
                if (start.isPartOfPath)
                {
                    start.AddEdge(edge);
                }
                if (end.isPartOfPath)
                {
                    end.AddEdge(edge);
                }
            }
        }

        private Tuple<int, int> GetRandomRotateVertexIndexes()
        {
            return new(
                RandomNumberGenerator.GetInt32(0, rows_c) * 2 + 1,
                RandomNumberGenerator.GetInt32(0, cols_c) * 2 + 1
                );
        }

        private Dictionary<Vertex, Vertex> BuildMinOstov(Vertex start, int total)
        {
            var path = new Dictionary<Vertex, Vertex>();
            var visited = new HashSet<Vertex>();
            Dfs(start, visited, path, total);
            return path;
        }
        private void Dfs(Vertex start, HashSet<Vertex> visited, Dictionary<Vertex, Vertex> parent, int total)
        {
            Stack<Vertex> stack = new Stack<Vertex>();
            stack.Push(start);
            while (stack.Count > 0)
            {
                Vertex current = stack.Pop();
                if (visited.Contains(current))
                    continue;
                visited.Add(current);
                if (visited.Count == total)
                    return;

                foreach (Vertex neighbor in current.AllNeighbors)
                {
                    if (neighbor.isPartOfPath)
                    {
                        foreach (Vertex n_neighbor in neighbor.AllNeighbors)
                        {
                            if (!visited.Contains(n_neighbor) && n_neighbor.isRotateAble)
                            {
                                if (!parent.ContainsKey(n_neighbor))
                                {
                                    parent[neighbor] = current;
                                    parent[n_neighbor] = neighbor;
                                }
                                stack.Push(n_neighbor);
                            }
                        }
                    }
                }
            }
        }

        public void PrintAllEdges()
        {
            foreach (var line1 in graph)
            {
                foreach (Vertex v in line1)
                {
                    string s = $"{v.ind}: ";
                    foreach (Edge e in v.Edges.Keys)
                    {
                        s += $"{e.start.ind}-{e.end.ind}, ";
                    }
                    Debug.Log(s);
                }
            }
        }

        public void PrintAllNeib()
        {
            foreach (var line1 in graph)
            {
                foreach (var tmpp in line1)
                {
                    string s = "";
                    s += tmpp.ind.ToString() + (tmpp.isPartOfPath ? "t" : "f").ToString() + ':';
                    foreach (var v in tmpp.AllSortedNeighbours)
                    {
                        s += v.ind + " ";
                    }
                    Debug.Log(s);
                }
            }
        }

       
    }
}

