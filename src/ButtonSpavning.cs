using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelGenerator;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject edgePrefab;
    private VertexView[,] grid;
    public float spacing = 1.5f;
    public int rows = 5;
    public int cols = 5;
    private LevelCreator levelCreator;
    private List<Vertex> graph;

    void Start()
    {
        levelCreator = new LevelCreator();
        graph = levelCreator.Create(rows, cols);
        SpawnGrid();
    }

    void SpawnGrid()
    {
        grid = new VertexView[rows, cols];
        Vector3 topLeft1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector3 topLeft = new Vector3(topLeft1.x + 2 * spacing, topLeft1.y - 2 * spacing, 0);

        for (int j = 0; j < rows; j++)
        {
            for (int k = 0; k < cols; k++)
            {
                int i = j * rows + k;
                Vector3 pos = new Vector3(topLeft.x + graph[i].X * spacing, topLeft.y - graph[i].Y * spacing, 0);
                GameObject obj = Instantiate(buttonPrefab, pos, Quaternion.identity);
                VertexView tmp = obj.GetComponent<VertexView>();
                tmp.Init(graph[i]);

                grid[j, k] = tmp;
            }
        }
    }
}


/*
 * 
 * grid = new VertexView[rows, cols];
        Vector3 topLeft1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector3 topLeft = new Vector3(topLeft1.x + 2 * spacing, topLeft1.y - 2 * spacing, 0);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector3 pos = new Vector3(topLeft.x + graph[y * cols + x].X * spacing,
                    topLeft.y - graph[y * cols + x].Y * spacing, 0);
                // new Vector3(topLeft.x + x * spacing, topLeft.y - y * spacing, 0);
                GameObject obj = Instantiate(buttonPrefab, pos, Quaternion.identity);
                VertexView button = obj.GetComponent<VertexView>();
                button.name = $"Vertex_{y * cols + x}";
                grid[y, x] = button;

                Vertex vertex = graph[y * cols + x];                
                button.Init(vertex);
            }
        }
        // CreateEdges();
 void CreateEdges()
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                ClickAbleButton a = grid[y, x];

                if (x + 1 < cols) Connect(a, grid[y, x + 1]);
                if (y + 1 < rows) Connect(a, grid[y + 1, x]);
            }
        }
    }

    void Connect(ClickAbleButton a, ClickAbleButton b)
    {
        if (edgePrefab == null)
        {
            Debug.LogError("Edge prefab не назначен!");
            return;
        }

        GameObject e = Instantiate(edgePrefab);

        EdgeView edgeComp = e.GetComponent<EdgeView>();
        if (edgeComp == null)
        {
            Debug.LogError("Prefab Edge не содержит скрипт Edge.cs!");
            return;
        }

        if (edgeComp.line == null)
        {
            Debug.LogError("LineRenderer в Edge не назначен!");
            return;
        }

        a.neighbors.Add(b);
        b.neighbors.Add(a);

        edgeComp.SetPoints(a.transform.position, b.transform.position);
    }
*/