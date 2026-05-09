using Alg1;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VertexView1 : MonoBehaviour
{
    static readonly int BloomingHash = Animator.StringToHash("Blooming");

    public GameObject edgePrefab;
    public Vertex vertex;
    public EventHandler ClickedEvent;
    [SerializeField] GameObject ground;
    bool isRotating;
    readonly List<GameObject> edges = new();

    private void Update()
    {
        if (isRotating)
            transform.Rotate(Vector3.up * 100f * Time.deltaTime);
    }

    public void UpdatePos()
    {
        transform.position = new Vector3(vertex.X, 0, vertex.Y);
    }

    public void Init(Vertex v)
    {
        vertex = v;
        UpdatePos();
        CreateEdges();
    }

    public void CreateEdges()
    {
        if (!vertex.isRotateAble) { return; }
        foreach (Edge edge in vertex.Edges.Keys)
        {
            GameObject edge_obj = Instantiate(edgePrefab);
            edges.Add(edge_obj);
            edge_obj.GetComponent<EdgeView1>().Init(edge);
        }
    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked!!" + vertex.ind.ToString());
        vertex.ClickAction();
        ClickedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void LevelFinished(object sender, EventArgs e)
    {
        isRotating = true;
        GetComponentInChildren<Animator>().Play(BloomingHash);
        if (ground != null) ground.SetActive(false);
        foreach (GameObject edge in edges)
            if (edge != null) edge.SetActive(false);
    }
}
