using Alg2.Domains;
using System.Collections.Generic;

namespace Alg2.Interfaces
{
    public class GetToFlowCalculator
    {
        DelaunatorUser levelCreator;
        IMaxFlowCalculator calculator;
        IFlowAmountGenerator gen;
        public GetToFlowCalculator(DelaunatorUser tmp, IMaxFlowCalculator calc, IFlowAmountGenerator gen1)
        {
            levelCreator = tmp;
            calculator = calc;
            gen = gen1;
        }

        public void UploadEdges(HashSet<Vertex> sources, HashSet<Vertex> targets)
        {
            foreach (var key in levelCreator.Graph.Keys)
            {
                Vertex from = levelCreator.Graph[key];
                foreach (Vertex neib in from.AllNeighbours)
                {
                    var flowAmount = gen.Next();
                    // if the edge connect 2 sources or 2 targets, we skip it
                    if ((targets.Contains(from) && targets.Contains(neib)) || 
                        (sources.Contains(from) && sources.Contains(neib)))
                        continue;
                    // if the edge leads to the source or from the target,
                    // we reverse it
                    else if (targets.Contains(from) || sources.Contains(neib))
                        calculator.AddEdge(neib, from, flowAmount);
                    
                    else
                        calculator.AddEdge(from, neib, flowAmount);
                }
            }
        }
    }
}
