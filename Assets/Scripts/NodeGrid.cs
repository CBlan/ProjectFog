using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour {

    //public Transform player;
    public LayerMask unwalkableMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    //public TerrainType[] walkableRegions;
    public int weightBlurSize = 2;
    public LayerMask walkableMask;
    public int walkableMaskMomentPenalty;
    public int obsticleProximityPenalty = 1;
    //Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    Node[,,] grid;

    public bool visualiseVolume;
    public bool visualiseCollision;

    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;

    //int penaltyMin = int.MaxValue;
    //int penaltyMax = int.MinValue;

    //public static NodeGrid nodeGrid;

    private void Awake()
    {
        //nodeGrid = this;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);

        //foreach (TerrainType region in walkableRegions)
        //{
        //    walkableMask.value |= region.terrainMask.value;
        //    walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        //}

        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY * gridSizeZ;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 worldBottomBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.up * gridWorldSize.y / 2) - (Vector3.forward * gridWorldSize.z / 2);
        

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius)) + (Vector3.up * (y * nodeDiameter + nodeRadius)) + (Vector3.forward * (z * nodeDiameter + nodeRadius));
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                    int movementPenalty = 0;

                    //checksphere
                    if (walkable)
                    {
                        //Ray ray = new Ray(worldPoint, Vector3.up); 
                        //RaycastHit hit;
                        //if (Physics.SphereCast(ray, nodeRadius, out hit, walkableMask))
                        //{
                        //    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                        //    //print(hit.collider.gameObject.layer + " : " + movementPenalty);
                        //}

                        if (Physics.CheckSphere(worldPoint, nodeRadius, walkableMask))
                        {
                            movementPenalty = walkableMaskMomentPenalty;
                        }
                    }

                    if (!walkable)
                    {
                        movementPenalty += obsticleProximityPenalty;
                    }

                    grid[x, y, z] = new Node(walkable, worldPoint, x, y, z, movementPenalty);
                }
            }
        }

        BlurPenaltyMap(weightBlurSize);
    }

    void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,,] penaltiesHorisontalPass = new int[gridSizeX, gridSizeY, gridSizeZ];
        int[,,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY, gridSizeZ];
        int[,,] penaltiesDepthPass = new int[gridSizeX, gridSizeY, gridSizeZ];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                for (int x = -kernelExtents; x <= kernelExtents; x++)
                {
                    int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                    penaltiesHorisontalPass[0, y, z] += grid[sampleX, y, z].movmentPenalty;
                }

                for (int x =1; x < gridSizeX; x++)
                {
                    int removeIndex = Mathf.Clamp( x - kernelExtents - 1, 0, gridSizeX);
                    int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                    penaltiesHorisontalPass[x, y, z] = penaltiesHorisontalPass[x-1, y, z] - grid[removeIndex, y, z].movmentPenalty + grid[addIndex, y, z].movmentPenalty;
                }
            }
        }


        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                for (int y = -kernelExtents; y <= kernelExtents; y++)
                {
                    int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                    penaltiesVerticalPass[x, 0, z] += penaltiesHorisontalPass[x, sampleY, z];
                }

                for (int y = 1; y < gridSizeY; y++)
                {
                    int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                    int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                    penaltiesVerticalPass[x, y, z] = penaltiesVerticalPass[x, y - 1, z] - penaltiesHorisontalPass[x, removeIndex, z] + penaltiesHorisontalPass[x, addIndex, z];
                }
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = -kernelExtents; z <= kernelExtents; z++)
                {
                    int sampleZ = Mathf.Clamp(z, 0, kernelExtents);
                    penaltiesDepthPass[x, y, 0] += penaltiesHorisontalPass[x, y, sampleZ];
                }

                for (int z = 1; z < gridSizeZ; z++)
                {
                    int removeIndex = Mathf.Clamp(z - kernelExtents - 1, 0, gridSizeZ);
                    int addIndex = Mathf.Clamp(z + kernelExtents, 0, gridSizeZ - 1);

                    penaltiesDepthPass[x, y, z] = penaltiesDepthPass[x, y, z - 1] - penaltiesVerticalPass[x, y, removeIndex] + penaltiesVerticalPass[x, y, addIndex];

                    int blurredPenalty = Mathf.RoundToInt((float)penaltiesDepthPass[x, y, z] / (kernelSize * kernelSize));
                    grid[x, y, z].movmentPenalty = blurredPenalty;

                    //if (blurredPenalty > penaltyMax)
                    //{
                    //    penaltyMax = blurredPenalty;
                    //}
                    //if (blurredPenalty < penaltyMin)
                    //{
                    //    penaltyMin = blurredPenalty;
                    //}
                }
            }
        }

    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ)
                    {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }

        return neighbours;
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
                foreach (Node n in grid)
                {
                    //Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movmentPenalty));

                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }

    }

    //[System.Serializable]
    //public class TerrainType
    //{
    //    public LayerMask terrainMask;
    //    public int terrainPenalty;
    //}
}
