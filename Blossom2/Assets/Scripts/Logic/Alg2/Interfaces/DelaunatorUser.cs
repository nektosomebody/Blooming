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

        private void CreateLevel()
        {
            CreateGraph(CreatePoints());
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