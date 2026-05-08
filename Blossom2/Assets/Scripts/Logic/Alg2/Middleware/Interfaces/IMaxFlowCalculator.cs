using Alg2.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg2.Interfaces
{
    public interface IMaxFlowCalculator
    {
        public int GetINF();
        public int MaxFlow(Vertex s, Vertex t);
        public void AddEdge(Vertex from, Vertex to, int cap);
    }
}
