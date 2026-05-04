using DelaunatorSharp;
using Alg2.Domains;
using Alg2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Alg2
{
    public class FinalAlgorithm: MonoBehaviour
    {
        [SerializeField] int n = 6;
        [SerializeField] float width = 100f;
        [SerializeField] float height = 100f;

        int minFlowAmount = 3;
        int maxFlowAmount = 15;
        int dim = 2;
        
        int countVerts;
        DelaunatorUser user;
        Dinic flowCalculator;
        List<Vertex> sources;
        List<Vertex> targets;

        public void Awake()
        {
            GenerateLevel();
        }
        
        public void GenerateLevel()
        {
            countVerts = n;
            user = new(n, height, width, new SobolSequence(dim));
            // 2 more because need 2 addition for multisource alg
            flowCalculator = new(n + 2);

            GetToFlowCalculator tmp = new(user, flowCalculator, new RandomIntGenerator(minFlowAmount, maxFlowAmount));
            
            GenerateSourcesAndSincs();
            tmp.UploadEdges(sources, targets);

            Debug.Log($"{sources[0]} {sources[1]} {targets[0]} {targets[1]}");
        }

        private void GenerateSourcesAndSincs(int countSources = 2, int countTargets = 2)
        {
            if (n < countSources + countTargets + 1 || n / 2 < countSources || n / 2 < countTargets)
            {
                throw new Exception("Lack of vertexes");
            }
            RandomIntGenerator genSources = new(0, countVerts / 2 - 1);
            RandomIntGenerator genTargets = new(countVerts / 2, countVerts - 1);
            HashSet<Vertex> tmpS = new();
            HashSet<Vertex> tmpT = new();
            List<Vertex> all_vertexes = GetAllVertexes();

            while (tmpS.Count < countSources)
            {
                tmpS.Add(all_vertexes[genSources.Next()]);
            }
            while (tmpT.Count < countTargets)
            {
                tmpT.Add(all_vertexes[genTargets.Next()]);
            }
            sources = tmpS.ToList();
            targets = tmpT.ToList();
        }

        public int GetMaxFlow()
        {
            return MultiSourceDinic.MaxFlowMulti(
            countVerts,
            sources,
            targets,
            flowCalculator);
        }

        public List<Vertex> GetAllVertexes()
        {
            return user.Graph.Values.ToList();
        }

        public List<EdgeWithFlow>[] GetEdges()
        {
            return flowCalculator.GetEdges();
        }
        
    }
}
