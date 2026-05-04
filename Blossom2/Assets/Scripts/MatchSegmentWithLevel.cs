using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alg1;

public class MatchSegmentWithLevel : MonoBehaviour
{
    [SerializeField] private float padding = 4f;
    [SerializeField] public GameObject Floor;
    [SerializeField] private float tree_padding = 0.05f;
    [SerializeField] GameObject Tree;
    public void ResizeFloor(Bounds bounds)
    {
        if (Floor == null)
        {
            Debug.LogError("Floor is not assigned!");
            return;
        }

        Vector3 levelSize = bounds.size + new Vector3(padding, 0, padding);

        Renderer rend = Floor.GetComponent<MeshRenderer>();
        if (rend == null)
        {
            Debug.LogError("Floor has no Renderer!");
            return;
        }

        Vector3 floorSize = rend.bounds.size;

        float scaleX = levelSize.x / floorSize.x;
        float scaleZ = levelSize.z / floorSize.z;

        Floor.transform.localScale = new Vector3(
            Floor.transform.localScale.x * scaleX,
            Floor.transform.localScale.y,
            Floor.transform.localScale.z * scaleZ
        );

        Floor.transform.position = bounds.center + new Vector3(0, -1.7f, 0);

        Vector3 size = Floor.transform.localScale;
        float marginX = size.x * tree_padding;
        float marginZ = size.z * tree_padding;

        float frst = -1;
        for (int i = 0; i < 2; i++)
        {
            frst *= -1;
            float scnd = -1;
            for (int j = 0; j < 2; j++)
            {
                scnd *= -1;
                Vector3 tmp = Floor.transform.position + new Vector3(frst * size.x / 2 - frst * marginX, size.y, scnd * size.z / 2 - scnd * marginZ);
                GameObject obj = Instantiate(Tree, tmp, Quaternion.identity);
            }
        }
    }
}
