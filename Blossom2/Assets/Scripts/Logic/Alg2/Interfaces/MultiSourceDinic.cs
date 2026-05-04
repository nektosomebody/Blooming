using Alg2.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg2.Interfaces
{
    public static class MultiSourceDinic
    {
        public static int MaxFlowMulti(
            int n,
            List<Vertex> sources,
            List<Vertex> sinks,
            IMaxFlowCalculator flowCalc
            )
        {
            Vertex superSource = new(n);
            Vertex superSink = new(n+1);


            foreach (var s in sources)
                flowCalc.AddEdge(superSource, s, flowCalc.GetINF());

            foreach (var t in sinks)
                flowCalc.AddEdge(t, superSink, flowCalc.GetINF());

            return flowCalc.MaxFlow(superSource, superSink);
        }
    }
}
