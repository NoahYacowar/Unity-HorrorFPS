using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    //Reference to the requestManager and grid
    PathRequestManager requestManager;
    Grid grid;

    void Awake()
    {
        //Reference to the requestManager and grid
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    //Pre:
    //Post:
    //Desc: Initializes finding path process
    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    //Pre:
    //Post:
    //Desc: Used to find path
    IEnumerator FindPath(Vector3 startingPos, Vector3 targetPos)
    {
        //Creating storage for waypoints and whether path is successful
        Vector3[] waypoints = new Vector3[0];
        bool pathSucess = false;

        //Creating a start and end Node within grid
        PathNode startNode = grid.NodeFromWorldPoint(startingPos);
        PathNode targetNode = grid.NodeFromWorldPoint(targetPos);

        //Determines whether both the start and end node are walkable
        if (startNode.walkable && targetNode.walkable)
        {
            //Creates an open and closed set for nodes that have been/are being checked
            Heap<PathNode> openSet = new Heap<PathNode>(grid.MaxSize);
            HashSet<PathNode> closedSet = new HashSet<PathNode>();
            openSet.Add(startNode);

            //While there are items to check
            while (openSet.Count > 0)
            {
                //Replacing node in OPEN with lowest f-cost
                PathNode curNode = openSet.RemoveFirst();
                closedSet.Add(curNode);

                //Determines whether the current node is equal to the target
                if (curNode == targetNode)
                {
                    //Letting program know path has been found
                    pathSucess = true;
                    break; 
                }

                //Loops through each node neighbouring the current node
                foreach (PathNode neighbour in grid.GetNeighbours(curNode))
                {
                    //Determines whether the neighbour is unusable
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    //Measuring cost of moving to neighbour
                    int newMovementCostToNeighbour = curNode.gCost + GetDistance(curNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        //Calculating neighbour's node values and setting parent equal to current node
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = curNode;

                        //Adding neighbour to the open set, or rearranging within heap 
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        //delays one from before returning;
        yield return null;
        if(pathSucess) waypoints = RetracePath(startNode, targetNode);
        requestManager.FinishedProcessingPath(waypoints, pathSucess);
    }

    //Pre:
    //Post:
    //Desc: Used to retrace path
    Vector3[] RetracePath(PathNode start, PathNode end)
    {
        //Creating pathList for storage, starting from final node
        List<PathNode> path = new List<PathNode>();
        PathNode curNode = end;

        //Looping until start has been found
        while(curNode != start)
        {
            //Adding current node to the path, equating current node to its parent
            path.Add(curNode);
            curNode = curNode.parent;
        }
        //Returning the path
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    //Pre:
    //Post:
    //Desc: Used to find path
    Vector3[] SimplifyPath(List<PathNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDir = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 newDir = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(newDir != oldDir)
            {
                waypoints.Add(path[i].worldPosition);
            }
            oldDir = newDir;
        }
        return waypoints.ToArray();
    }

    //Pre:
    //Post:
    //Desc: Calculates distance between nodes
    int GetDistance(PathNode A, PathNode B)
    {
        //Differences in x & y
        int distanceX = Mathf.Abs(A.gridX - B.gridX);
        int distanceY = Mathf.Abs(A.gridY - B.gridY);

        //Determines whether delta x is greater than delta y
        if(distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
