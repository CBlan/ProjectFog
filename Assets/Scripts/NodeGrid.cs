using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour {

    //public Transform player;
    public LayerMask unwalkableMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    Node[,,] grid;
    public bool visualiseVolume;
    public bool visualiseCollision;

    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 worldBottomBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.up * gridWorldSize.y / 2) - (Vector3.forward * gridWorldSize.z / 2);
        //Vector3 worldTopBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.up * gridWorldSize.y / 2) + (Vector3.forward * gridWorldSize.z / 2);


        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius)) + (Vector3.up * (y * nodeDiameter + nodeRadius)) + (Vector3.forward * (z * nodeDiameter + nodeRadius));
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    grid[x, y, z] = new Node(walkable, worldPoint);
                }
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        float percentZ = (worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return grid[x, y, z];
    }

    private void OnDrawGizmos()
    {
        if (visualiseVolume)
        {
            Gizmos.DrawWireCube(transform.position, gridWorldSize);
        }

        if (visualiseCollision)
        {
            if (grid != null)
            {
                //Node playerNode = NodeFromWorldPoint(player.position);
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.green : Color.red;
                    //if (playerNode == n)
                    //{
                    //    Gizmos.color = Color.blue;
                    //}
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }

    }
}
