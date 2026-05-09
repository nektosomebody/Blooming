using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alg2.Domains;
using Alg2.Interfaces;
using Alg2;

public class Spawner2: MonoBehaviour
{
    [SerializeField] GameObject edgePrefab;
    [SerializeField] GameObject middleVertexPrefab;
    [SerializeField] GameObject sourcePrefab;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] GameObject flowPrefab;
    [SerializeField] GameObject edgeStartPrefab;
    [SerializeField] GameObject edgeEndPrefab;
    [SerializeField] float vertexRadius = 5f;
    [SerializeField] float yPos = 0f;
    [SerializeField] int minVertexCapacity = 5;
    [SerializeField] int maxVertexCapacity = 20;
    LevelResultManager resultManager;
    FinalAlgorithm flowAlg;
    private void Start()
    {
        flowAlg = GetComponent<FinalAlgorithm>();
        resultManager = GetComponent<LevelResultManager>();
        SpawnLevel();       
    }
    private void SpawnLevel()
    {
        List<Vertex> all_vertices = flowAlg.GetAllVertexes();
        all_vertices.Sort((a, b) => a.CompareTo(b));

        HashSet<TargetVertex> targetVertices = new();
        HashSet<MiddleVertexView> middleVertices = new();
        Dictionary<int, VertexViewParent> vertexViewMap = new();
        
        // instantiate vertices
        foreach (Vertex vertDomain in all_vertices)
        {
            Vector3 pos = new(vertDomain.x, yPos, vertDomain.y);
            GameObject obj;
            int capacity = Random.Range(minVertexCapacity, maxVertexCapacity + 1);

            if (flowAlg.Sources.Contains(vertDomain))
            {
                obj = Instantiate(sourcePrefab, pos, Quaternion.identity);
            }
            else if (flowAlg.Targets.Contains(vertDomain))
            {
                obj = Instantiate(targetPrefab, pos, Quaternion.identity);
                targetVertices.Add(obj.GetComponent<TargetVertex>());
            }
            else
            {
                obj = Instantiate(middleVertexPrefab, pos, Quaternion.identity);
                flowAlg.SetVertexCapacity(vertDomain.ind, capacity);
            }

            VertexViewParent view = obj.GetComponent<VertexViewParent>();
            view.Init(vertDomain, capacity);
            vertexViewMap[vertDomain.ind] = view;
            if (view is MiddleVertexView middleView)
                middleVertices.Add(middleView);
        }
        resultManager.Init(targetVertices, middleVertices, flowAlg.GetMaxFlow());
        GetComponent<LevelFinished>().Init(resultManager);
        var all_edges = flowAlg.GetEdges();
        /*
        because all_vertexes contains all the vertices that are on the field, 
        and all_edges also includes 2 additional ones for the algorithm to work correctly
        */
        for (int i = 0; i < all_vertices.Count; i++)
        {
            Vertex from = all_vertices[i];
            foreach (EdgeWithFlow edgeDomain in all_edges[i])
            {
                if (!vertexViewMap.ContainsKey(edgeDomain.To.ind)) continue;
                Vector3 posFrom = new(from.x, yPos, from.y);
                Vertex to = edgeDomain.To;
                Vector3 posTo = new(to.x, yPos, to.y);
                Vector3 pos = (posTo + posFrom) / 2;

                Vector3 dir = posTo - posFrom;
                Quaternion rot = Quaternion.LookRotation(dir.normalized);

                GameObject obj = Instantiate(edgePrefab, pos, rot);
                GameObject flow = Instantiate(flowPrefab, pos - dir / 2, rot);
                Vector3 offset = dir.normalized * vertexRadius;
                Instantiate(edgeStartPrefab, posFrom + offset, rot);
                Instantiate(edgeEndPrefab, posTo - offset, rot);

                float length = Vector3.Distance(posFrom, posTo);
                Vector3 scale = obj.transform.localScale;
                scale.z = length;
                obj.transform.localScale = scale;

                EdgeWithFlowView view = obj.GetComponent<EdgeWithFlowView>();
                // important to set lenght after changing the scale
                Debug.Log($"{obj.transform.localScale} {length}");
                view.Init(edgeDomain, length, flow, vertexViewMap[from.ind], posFrom, posTo);
                flowAlg.cleanFlowEvent += view.ResetFlow;
                if (vertexViewMap.TryGetValue(edgeDomain.To.ind, out var toView))
                {
                    view.FlowIncreased += toView.IncreaseFlow;
                    if (toView is MiddleVertexView middleToView)
                        view.FlowDecreased += middleToView.OnFlowDecreased;
                }
            }
        }
    }
}
