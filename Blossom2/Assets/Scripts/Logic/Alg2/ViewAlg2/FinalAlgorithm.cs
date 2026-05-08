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
        [SerializeField] private float Width = 100f;
        [SerializeField] private float Height = 100f;
        [SerializeField] private int countSources = 2;
        [SerializeField] private int countTargets = 2;
        [SerializeField] Camera cam;

        [SerializeField] int minFlowAmount = 3;
        [SerializeField] int maxFlowAmount = 15;
        public EventHandler cleanFlowEvent;
        int dim = 2;        
        int countVerts;
        DelaunatorUser user;
        Dinic flowCalculator;
        Dictionary<int, int> vertexCapacities = new();
        private HashSet<Vertex> sources = new();  
        private HashSet<Vertex> targets = new();       
        public HashSet<Vertex> Sources
        {
            get
            {
                if (sources == null || sources.Count == 0)
                    throw new Exception("Sources not generated yet");
                return sources;
            }
            private set => sources = value;
        }
        public HashSet<Vertex> Targets
        {
            get
            {
                if (targets == null || targets.Count == 0)
                    throw new Exception("Targets not generated yet");
                return targets;
            }
            private set => targets = value;
        }
        

        public void Awake()
        {
            GenerateLevel();
            
        }
        public void Start()
        {
            cam.GetComponent<CameraFitter>().FitCamera(Width, Height, new Vector3(Width / 2, transform.position.y, Height / 2));
        }

        
        
        public void GenerateLevel()
        {
            countVerts = n;
            user = new DelaunatorUser(n, Height, Width, new SobolSequence(dim));
            // 2 more because need 2 addition for multisource alg
            flowCalculator = new Dinic(n + 2);

            GetToFlowCalculator adapter = new(user, flowCalculator, new RandomIntGenerator(minFlowAmount, maxFlowAmount));
            
            GenerateSourcesAndSincs(countSources, countTargets);
            adapter.UploadEdges(sources, targets);

            Debug.Log($"{sources.ElementAt(0)} {sources.ElementAt(1)} {targets.ElementAt(0)} {targets.ElementAt(1)}");
        }

        private void GenerateSourcesAndSincs(int countSources, int countTargets)
        {
            if (n < countSources + countTargets + 1)
                throw new Exception("Lack of vertexes");

            List<Vertex> all_vertexes = GetAllVertexes();
            all_vertexes.Sort((a, b) => b.y.CompareTo(a.y));

            // top of screen
            Stack<Vertex> sourcesStack = new();
            for (int i = 0; i < countSources; i++)
                sources.Add(all_vertexes[i]); 

             // bottom of screen
            Stack<Vertex> targetsStack = new();
            for (int i = all_vertexes.Count - 1; i >= all_vertexes.Count - countTargets; i--)
                targets.Add(all_vertexes[i]);
        }

        public void SetVertexCapacity(int vertexInd, int capacity)
        {
            vertexCapacities[vertexInd] = capacity;
        }

        public int GetMaxFlow()
        {
            int splitN = countVerts * 2;
            Dinic splitGraph = new Dinic(splitN + 2);

            var sourceInds = new HashSet<int>(sources.Select(s => s.ind));
            var targetInds = new HashSet<int>(targets.Select(t => t.ind));
            var edges = flowCalculator.GetEdges();

            // copy edges with vertex splitting for middle vertices:
            // outgoing from middle vertex uses v_out (ind + countVerts), incoming uses v_in (original ind)
            for (int i = 0; i < countVerts; i++)
            {
                bool isMiddle = !sourceInds.Contains(i) && !targetInds.Contains(i);
                int fromInd = isMiddle ? i + countVerts : i;
                foreach (var edge in edges[i])
                    splitGraph.AddEdge(new Vertex(fromInd), new Vertex(edge.To.ind), edge.Cap);
            }

            // add internal v_in → v_out edges with vertex capacity for each middle vertex
            foreach (var kvp in vertexCapacities)
                splitGraph.AddEdge(new Vertex(kvp.Key), new Vertex(kvp.Key + countVerts), kvp.Value);

            int ans = MultiSourceDinic.MaxFlowMulti(splitN, sources, targets, splitGraph);
            cleanFlowEvent?.Invoke(this, EventArgs.Empty);
            return ans;
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
