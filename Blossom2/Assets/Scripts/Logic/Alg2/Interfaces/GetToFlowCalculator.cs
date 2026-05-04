using Alg2.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg2.Interfaces
{
    public class GetToFlowCalculator
    {
        DelaunatorUser levelCreator;
        IMaxFlowCalculator calculator;
        IFlowAmountGenerator gen;
        public GetToFlowCalculator(DelaunatorUser tmp, IMaxFlowCalculator dinic, IFlowAmountGenerator gen1)
        {
            levelCreator = tmp;
            calculator = dinic;
            gen = gen1;
        }

        public void UploadEdges(List<Vertex> sources, List<Vertex> targets)
        {
            foreach (var key in levelCreator.Graph.Keys)
            {
                // no edges should come out of the drains
                if (targets.Contains(levelCreator.Graph[key]))
                {
                    continue;
                }
                foreach (Vertex neib in levelCreator.Graph[key].AllNeighbours)
                {
                    // edges should not enter the sources
                    if (sources.Contains(neib))
                    {
                        continue;
                    }
                    var flowAmount = gen.Next();
                    calculator.AddEdge(levelCreator.Graph[key], neib, flowAmount);

                    Console.WriteLine($"{levelCreator.Graph[key].ind} ({levelCreator.Graph[key].x}, {levelCreator.Graph[key].y}) -> {neib.ind} {flowAmount}");
                }
            }
        }
    }
}
