using DelaunatorSharp;
using Alg2.Domains;
using System;
using System.Collections.Generic;


namespace Alg2.Interfaces
{
    public class DelaunatorUser
    {
        IPointGenerator pointGenerator;
        float maxX;
        float maxY;
        int countOfPoints;
        float distTreshold = 40f;
        private const float maxObtuseAngleDeg = 165f;
        public Dictionary<(float, float), Vertex> Graph { get; private set; }

        public DelaunatorUser(int n, float height, float width, IPointGenerator generator)
        {
            pointGenerator = generator;
            countOfPoints = n;
            maxX = width;
            maxY = height;

            CreateLevel();
        }
        private void CreateLevel()
        {
            IPoint[] points = ToIPoints(CreatePoints());
            Delaunator delaunator = CreateGraph(points);
            FilterObtuseTriangles(points, delaunator, maxObtuseAngleDeg);
        }
        private float CalcDist(float[] p1, float[] p2)
        {
            return MathF.Sqrt((p1[0] - p2[0]) * (p1[0] - p2[0]) + (p1[1] - p2[1]) * (p1[1] - p2[1]));
        }

        private IPoint[] ToIPoints(List<float[]> points)
        {
            IPoint[] iPoints = new IPoint[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                iPoints[i] = new Point(points[i][0], points[i][1]);
            }
            return iPoints;
        }
        private List<float[]> CreatePoints()
        {
            List<float[]> points = new List<float[]>();
            int attempts = 0;
            const int maxAttempts = 10000;
            while (points.Count < countOfPoints && attempts++ < maxAttempts)
            {
                float[] tmp = pointGenerator.Next();
                tmp[0] *= maxX;
                tmp[1] *= maxY;
                bool correct = true;
                foreach (var point in points)
                {
                    if (CalcDist(tmp, point) < distTreshold)
                    {
                        correct = false;
                        break;
                    }
                }
                if (!correct)
                {
                    continue;
                }
                points.Add(tmp);
            }
            return points;
        }

        private Delaunator CreateGraph(IPoint[] points)
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

            return delaunator;
        }
        private void FilterObtuseTriangles(IPoint[] points, Delaunator delaunator, float maxAngleDeg)
        {
            for (int i = 0; i < delaunator.Triangles.Length; i += 3)
            {
                IPoint pa = points[delaunator.Triangles[i]];
                IPoint pb = points[delaunator.Triangles[i + 1]];
                IPoint pc = points[delaunator.Triangles[i + 2]];

                if (AngleBetween(pa, pb, pc) > maxAngleDeg) TryRemoveEdge(pb, pc);
                if (AngleBetween(pb, pa, pc) > maxAngleDeg) TryRemoveEdge(pa, pc);
                if (AngleBetween(pc, pa, pb) > maxAngleDeg) TryRemoveEdge(pa, pb);
            }
        }

        private void TryRemoveEdge(IPoint p, IPoint q)
        {
            var kP = ((float)p.X, (float)p.Y);
            var kQ = ((float)q.X, (float)q.Y);
            if (Graph.TryGetValue(kP, out var vP) && Graph.TryGetValue(kQ, out var vQ))
            {
                vP.RemoveFromAllNeighbours(vQ);
                vQ.RemoveFromAllNeighbours(vP);
            }
        }

        private static float AngleBetween(IPoint v, IPoint p, IPoint q)
        {
            float dx1 = (float)(p.X - v.X), dy1 = (float)(p.Y - v.Y);
            float dx2 = (float)(q.X - v.X), dy2 = (float)(q.Y - v.Y);
            float dot = dx1 * dx2 + dy1 * dy2;
            float len = MathF.Sqrt((dx1 * dx1 + dy1 * dy1) * (dx2 * dx2 + dy2 * dy2));
            if (len < 1e-10f) return 0f;
            float cosA = dot / len;
            if (cosA < -1f) cosA = -1f;
            if (cosA > 1f) cosA = 1f;
            return MathF.Acos(cosA) * (180f / MathF.PI);
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