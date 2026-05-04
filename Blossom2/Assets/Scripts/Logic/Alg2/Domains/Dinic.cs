using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alg2.Interfaces;

namespace Alg2.Domains
{
    class Dinic: IMaxFlowCalculator
    { 
        public List<EdgeWithFlow>[] Graph { get; private set; }
        private int[] level;
        private int[] ptr;
        private readonly int n;

        internal const int INF = int.MaxValue;

        public Dinic(int n)
        {
            this.n = n;
            Graph = new List<EdgeWithFlow>[n];
            for (int i = 0; i < n; i++)
                Graph[i] = new List<EdgeWithFlow>();

            level = new int[n];
            ptr = new int[n];
        }
        public int GetINF()
        {
            return INF;
        }
        public List<EdgeWithFlow>[] GetEdges()
        {
            var edgesWithoutBackwards = new List<EdgeWithFlow>[Graph.Length];
            for (int i = 0; i < Graph.Length; i++)
            {
                edgesWithoutBackwards[i] = new();
                foreach (var edge in Graph[i]) {
                    if (edge.Cap == 0)
                    {
                        continue;
                    }
                    edgesWithoutBackwards[i].Add(edge);
                }
            }

            return edgesWithoutBackwards;

        }
        public List<EdgeWithFlow>[] GetCopyOfEdges()
        {
            var copy = new List<EdgeWithFlow>[Graph.Length];

            for (int i = 0; i < Graph.Length; i++)
            {
                copy[i] = new List<EdgeWithFlow>();

                foreach (var edge in Graph[i])
                {
                    copy[i].Add(new EdgeWithFlow(edge));
                }
            }

            return copy;
        }

        public void AddEdge(Vertex from, Vertex to, int cap)
        {
            var forward = new EdgeWithFlow(to, cap, Graph[to.ind].Count);
            var backward = new EdgeWithFlow(from, 0, Graph[from.ind].Count);

            Graph[from.ind].Add(forward);
            Graph[to.ind].Add(backward);
        }

        public int MaxFlow(Vertex sourse, Vertex target)
        {
            int s = sourse.ind;
            int t = target.ind;
            int flow = 0;

            while (BFS(s, t))
            {
                Array.Fill(ptr, 0);

                while (true)
                {
                    int pushed = DFS(s, t, INF);
                    if (pushed == 0)
                        break;

                    flow += pushed;
                }
            }

            return flow;
        }

        private bool BFS(int s, int t)
        {
            Array.Fill(level, -1);
            level[s] = 0;

            Queue<int> q = new Queue<int>();
            q.Enqueue(s);

            while (q.Count > 0)
            {
                int v = q.Dequeue();

                foreach (var e in Graph[v])
                {
                    if (e.Cap - e.Flow > 0 && level[e.To.ind] == -1)
                    {
                        level[e.To.ind] = level[v] + 1;
                        q.Enqueue(e.To.ind);
                    }
                }
            }
            return level[t] != -1;
        }

        private int DFS(int v, int t, int pushed)
        {
            if (pushed == 0)
                return 0;

            if (v == t)
                return pushed;

            for (; ptr[v] < Graph[v].Count; ptr[v]++)
            {
                var e = Graph[v][ptr[v]];

                if (level[e.To.ind] != level[v] + 1 || e.Cap - e.Flow <= 0)
                    continue;

                int tr = DFS(e.To.ind, t, Math.Min(pushed, e.Cap - e.Flow));

                if (tr == 0)
                    continue;

                e.Flow += tr;
                Graph[e.To.ind][e.Rev].Flow -= tr;

                return tr;
            }
            return 0;
        }
    }
}
