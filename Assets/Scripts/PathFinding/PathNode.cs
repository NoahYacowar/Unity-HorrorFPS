using System.Collections;
using System;
using UnityEngine;

public class PathNode : IHeapItem<PathNode>
{
    //Various data stored in each PathNode
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public PathNode parent;
    int heapIndex;

    //Pre:
    //Post:
    //Desc: Constructor for PathNode object
    public PathNode(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    //Determines fCost of node (overall cost)
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //Can both set and get heapIndex value
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    //Pre: Valid/Correct PathNode
    //Post:
    //Desc: Compares the values of this node and another specified
    public int CompareTo(PathNode comparisonNode)
    {
        //Compares the nodes
        int compare = fCost.CompareTo(comparisonNode.fCost);
        if (compare == 0) compare = hCost.CompareTo(comparisonNode.hCost);

        //Determines (returns) which of the Nodes has a smaller value
        return -compare;
    }
}
