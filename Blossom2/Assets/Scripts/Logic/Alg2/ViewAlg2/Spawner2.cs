using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Alg2.Domains;
using Alg2.Interfaces;
using Alg2;

public class Spawner2: MonoBehaviour
{
    [SerializeField] GameObject edgePrefab;
    [SerializeField] GameObject vertexPrefab;
    [SerializeField] GameObject flowPrefab;
    [SerializeField] float yPos = 0f;
    FinalAlgorithm flowAlg;
    private void Start()
    {
        flowAlg = GetComponent<FinalAlgorithm>();
        SpawnLevel();
    }
    private void SpawnLevel()
    {
        List<Vertex> all_vertices = flowAlg.GetAllVertexes();
        all_vertices.Sort((a, b) => a.CompareTo(b));

        foreach (Vertex vertDomain in all_vertices)
        {
            Vector3 pos = new(vertDomain.x, yPos, vertDomain.y);
            GameObject obj = Instantiate(vertexPrefab, pos, Quaternion.identity);
            VertexView2 view = obj.GetComponent<VertexView2>();
            view.Init(vertDomain);
        }
        
        var all_edges = flowAlg.GetEdges();
        /*
        because all_vertexes contains all the vertices that are on the field, 
        and all_edges also includes 2 additional ones for the algorithm to work correctly.
        */
        for (int i = 0; i < all_vertices.Count; i++)
        {
            Vertex from = all_vertices[i];
            foreach (EdgeWithFlow edgeDomain in all_edges[i])
            {
                Vector3 posFrom = new(from.x, yPos, from.y);
                Vertex to = edgeDomain.To;
                Vector3 posTo = new(to.x, yPos, to.y);
                Vector3 pos = (posTo + posFrom) / 2;

                Vector3 dir = posTo - posFrom;
                Quaternion rot = Quaternion.LookRotation(dir.normalized);

                GameObject obj = Instantiate(edgePrefab, pos, rot);
                GameObject flow = Instantiate(flowPrefab, pos - dir / 2, rot);

                float length = Vector3.Distance(posFrom, posTo);
                Vector3 scale = obj.transform.localScale;
                scale.z = length;
                obj.transform.localScale = scale;

                EdgeView2 view = obj.GetComponent<EdgeView2>();
                // important to set lenght after changing the scale
                Debug.Log($"{obj.transform.localScale} {length}");
                view.Init(edgeDomain, length, flow);
            }
            
        }
    }
}
