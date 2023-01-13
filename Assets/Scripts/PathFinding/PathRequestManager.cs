using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    //Generates Queue
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest curPathRequest;

    //Reference to pathfinding class
    static PathRequestManager instance;
    Pathfinding pathfinding;
    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    //Pre:
    //Post:
    //Desc: Called from unit class, requests path
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    //Pre:
    //Post:
    //Desc: Allows new path to be processed
    void TryProcessNext()
    {
        //Determines whether a path is already being processed and that there is a request in
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            //Dequeues pathrequest, calls startfindpath method on dequeued value
            curPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(curPathRequest.pathStart, curPathRequest.pathEnd);
        }

    }

    //Pre:
    //Post:
    //Desc: Called by pathfinding script once finished finding path
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        //Finished path, moves to next
        curPathRequest.callBack(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    //Simply a collection of data
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callBack;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callBack)
        {
            pathStart = start;
            pathEnd = end;
            this.callBack = callBack;
        }
    }
}
