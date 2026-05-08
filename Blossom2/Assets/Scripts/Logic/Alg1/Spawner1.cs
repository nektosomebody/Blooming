using Alg1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Spawner1 : MonoBehaviour
{
    LevelData levelData;
    public GameObject rotateAbleVert;
    // public GameObject notRotateAbleVert;
    public float spacing = 1.5f;
    public float zpos = 0f;
    private List<List<Vertex>> graph;

    void Start()
    {
        levelData = GetComponent<LevelData>();
        graph = levelData.Graph;
        SpawnGrid();
    }
    void SpawnGrid()
    {
        Vector3 topLeft1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector3 topLeft = new Vector3(topLeft1.x + 2 * spacing, topLeft1.y - 2 * spacing, 0);

        for (int j = 0; j < graph.Count; j++)
        {
            for (int k = 0; k < graph[j].Count; k++)
            {
                Vector3 pos = new Vector3(topLeft.x + graph[j][k].X * spacing, topLeft.y - graph[j][k].Y * spacing, zpos);

                if (!graph[j][k].isRotateAble) { continue; }
                
                GameObject obj = Instantiate(rotateAbleVert, pos, Quaternion.identity);
                //if (graph[j][k].isRotateAble) { obj = Instantiate(rotateAbleVert, pos, Quaternion.identity); }
                //else { obj = Instantiate(notRotateAbleVert, pos, Quaternion.identity); }

                VertexView1 tmp = obj.GetComponent<VertexView1>();
                if (graph[j][k].isRotateAble)
                {
                    // subscribing to an event to verify that the graph is connected again
                    tmp.ClickedEvent += levelData.CheckIfWin; 
                    levelData.playerWon += tmp.Rotate;
                    levelData.playerWon += tmp.PlayBloomingAnimation;
                }
                tmp.Init(graph[j][k]);
            }
        }
    }
}

