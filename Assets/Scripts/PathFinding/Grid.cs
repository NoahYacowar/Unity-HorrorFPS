using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : MonoBehaviour
{
    //Used for drawing grid
    public bool displayGridGizmo;

    //Used for determining characteristics of gird (size, walkables, etc...)
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    PathNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        //Determining node's diameter and sequentially quantity of nodes in both horizontal and verticle axis
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    //Determines the maximum quantity of nodes
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    //Pre: positive values greater than 0
    //Post:
    //Desc: Gives us the ability to determine number of grid elements
    public void SetDim(int x, int y)
    {
        //Determining real world size and quantity of nodes dependant on nodes horizontally and vertically
        gridWorldSize = new Vector2(x*nodeDiameter, y*nodeDiameter);
        gridSizeX = x;
        gridSizeY = y;
    }

    //Pre:
    //Post:
    //Desc: Resets Grid through determining all nodes to walkable
    public void ResetGrid()
    {
        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                grid[x, y].walkable = true;
            }
        }
    }

    //Pre:
    //Post:
    //Desc: Generates a grid, mapping areas of restrictiveness
    public void CreateGrid()
    {
        //Defining grid 
        grid = new PathNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        //Looping through all grid elements
        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Determining characteristics of node
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new PathNode(walkable, worldPoint, x, y);
            }
        }
    }

    //Pre:
    //Post:
    //Desc: Determines neighbours of PathNode
    public List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; //Skips this iteration

                //Determining position of new Node within grid
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //Testing to ensure valid neighbour has been found
                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    //Adding neighbours to return list
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    //Pre:
    //Post:
    //Desc: Determines a node within grid given a world position
    public PathNode NodeFromWorldPoint(Vector3 worldPos)
    {
        //Determining percentage of worldposition relative to grid
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        //Finding approximate node positions within grid (relating percentage to maximums)
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    //Pre:
    //Post:
    //Desc: Draws the grid for visualization (Not necessary!)
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        
        if(grid != null && displayGridGizmo)
        {
            foreach (PathNode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
