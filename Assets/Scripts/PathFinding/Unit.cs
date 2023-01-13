using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //Various references and storage for creating/referencing a path
    public Transform target;
    public float speed;
    Vector3[] path;
    int targetIndex;

    //Timer determining Enemy reaction time
    private float newPathTime;
    private float oldPathTime = 0;
    public float enemyReactivity = 0.3f;
    public float rotationSpeed = 0.3f;


    void Update()
    {       
        //Storing the current time elapsed 
        float newPathTime = Time.time;

        //Accessed once one second has passed
        if(newPathTime >= oldPathTime + enemyReactivity)
        {
            //Putting in request to find path to targer
            oldPathTime = newPathTime;
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }
    }

    //Pre:
    //Post:
    //Desc: Determines whether the found path is acceptable
    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if(pathSuccess)
        {
            //Setting the path equal to our newly found path
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }

    }


    IEnumerator FollowPath()
    {
        //Equating to first Vector3 within path array
        Vector3 curWaypoint = path[0];

        while (true)
        {
            //Determines whether parent object has successfull reached next node goal
            if (transform.position == curWaypoint)
            {
                //Target index of node in path (move to next)
                targetIndex++;

                //Determines whether path has been completed
                if (targetIndex >= path.Length)
                {
                    //Exit Coroutine
                    yield break;
                }

                //Setting next goal to the sequential path node
                curWaypoint = path[targetIndex];
            }

            //Moving the player
            transform.position = Vector3.MoveTowards(transform.position, curWaypoint, speed);
            transform.LookAt(target);

            //Moving to next frame
            yield return null;
        }
    }

    //Pre: Realistic increment
    //Post:
    //Desc: Increases move speed of parent object
    public void SpeedIncrement(float increment)
    {
        speed += increment;
    }
}
