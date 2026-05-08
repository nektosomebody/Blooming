using DelaunatorSharp;
using Alg2.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg2.Interfaces
{
    public class DelaunatorUser
    {
        IPointGenerator pointGenerator;
        double maxX;
        double maxY;
        int countOfPoints;
        public Dictionary<(float, float), Vertex> Graph { get; private set; }

        public DelaunatorUser(int n, float height, float width, IPointGenerator generator)
        {
            pointGenerator = generator;
            countOfPoints = n;
            maxX = width;
            maxY = height;

            CreateLevel();
        }

        private const float MinAngleDeg = 15f;

        private void CreateLevel()
        {
            CreateGraph(CreatePoints());
            FilterNarrowEdges(MinAngleDeg);
        }
        private IPoint[] CreatePoints()
        {
            IPoint[] points = new IPoint[countOfPoints];
            for (int i = 0; i < countOfPoints; i++)
            {
                float[] tmp = pointGenerator.Next();
                points[i] = new Point(tmp[0] * maxX, tmp[1] * maxY);
            }
            return points;
        }

        private void CreateGraph(IPoint[] points)
        {
            Delaunator delaunator = new Delaunator(points);

            Graph = new(new FloatPointComparer());
            int ind = 0;

            foreach (IEdge edge in delaunator.GetEdges())
            {
                var pKey = (Convert.ToSingle(edge.P.X), Convert.ToSingle(edge.P.Y));
                var qKey = (Convert.ToSingle(edge.Q.X), Convert.ToSingle(edge.Q.Y));

                if (!Graph.TryGetValue(pKey, out var vP))
                {
                    vP = new Vertex(ind, (float)edge.P.X, (float)edge.P.Y);
                    Graph[pKey] = vP;
                    ind++;
                }

                if (!Graph.TryGetValue(qKey, out var vQ))
                {
                    vQ = new Vertex(ind, (float)edge.Q.X, (float)edge.Q.Y);
                    Graph[qKey] = vQ;
                    ind++;
                }
                vP.AddToAllNeighbours(vQ);
            }
        }

        class FloatPointComparer : IEqualityComparer<(float, float)>
        {
            const int MULTIPLIER = 1000000;
            public bool Equals((float, float) a, (float, float) b)
            {
                return MathF.Abs(a.Item1 - b.Item1) < 1e-4f &&
                       MathF.Abs(a.Item2 - b.Item2) < 1e-4f;
            }

            public int GetHashCode((float, float) p)
            {
                int x = (int)(p.Item1 * MULTIPLIER);
                int y = (int)(p.Item2 * MULTIPLIER);
                return HashCode.Combine(x, y);
            }
        }

        private void FilterNarrowEdges(float minAngleDeg)
        {
            float minAngleRad = minAngleDeg * MathF.PI / 180f;

            foreach (var vertex in Graph.Values)
            {
                bool changed = true;
                while (changed)
                {
                    changed = false;
                    var neighbours = vertex.AllNeighbours.ToList();
                    if (neighbours.Count < 2) break;

                    neighbours.Sort((a, b) =>
                    {
                        float angleA = MathF.Atan2(a.y - vertex.y, a.x - vertex.x);
                        float angleB = MathF.Atan2(b.y - vertex.y, b.x - vertex.x);
                        return angleA.CompareTo(angleB);
                    });

                    int n = neighbours.Count;
                    for (int i = 0; i < n; i++)
                    {
                        Vertex n1 = neighbours[i];
                        Vertex n2 = neighbours[(i + 1) % n];

                        float a1 = MathF.Atan2(n1.y - vertex.y, n1.x - vertex.x);
                        float a2 = MathF.Atan2(n2.y - vertex.y, n2.x - vertex.x);
                        float diff = MathF.Abs(a2 - a1);
                        if (diff > MathF.PI) diff = 2f * MathF.PI - diff;

                        if (diff < minAngleRad)
                        {
                            Vertex toRemove = DistSq(vertex, n1) < DistSq(vertex, n2) ? n1 : n2;
                            vertex.RemoveFromAllNeighbours(toRemove);
                            changed = true;
                            break;
                        }
                    }
                }
            }
        }

        private static float DistSq(Vertex a, Vertex b)
        {
            float dx = a.x - b.x, dy = a.y - b.y;
            return dx * dx + dy * dy;
        }

        public void ShowGraph()
        {
            foreach (var key in Graph.Keys)
            {
                string s = $"{Graph[key].x} {Graph[key].y}:";
                foreach (Vertex v1 in Graph[key].AllNeighbours)
                {
                    s += $" {v1.x} {v1.y},";
                }
                Console.WriteLine(s);
            }
        }
    }
}