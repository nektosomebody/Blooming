using UnityEngine;

public class MatchSegmentWithLevel : MonoBehaviour
{
    [SerializeField] private float padding = 4f;
    [SerializeField] public GameObject Floor;
    [SerializeField] private float floorYOffset = -1.7f;

    public void ResizeFloor(Bounds bounds)
    {
        if (Floor == null) { Debug.LogError("Floor is not assigned!"); return; }

        Renderer rend = Floor.GetComponent<MeshRenderer>();
        if (rend == null) { Debug.LogError("Floor has no Renderer!"); return; }

        Vector3 levelSize = bounds.size + new Vector3(padding, 0, padding);
        Vector3 floorSize = rend.bounds.size;

        Floor.transform.localScale = new Vector3(
            Floor.transform.localScale.x * levelSize.x / floorSize.x,
            Floor.transform.localScale.y,
            Floor.transform.localScale.z * levelSize.z / floorSize.z
        );

        Floor.transform.position = new Vector3(bounds.center.x, floorYOffset, bounds.center.z);
    }
}
