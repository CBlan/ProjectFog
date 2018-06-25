using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    const float pathUpdateMoveThreshhold = 0.5f;
    const float minPathUpdateTime = 0.5f;

    public Transform target;
    public float speed = 20;

    public float turnDistance = 5;
    public float turnSpeed = 3;

    Vector3[] path;
    int targetIndex;

    private void Start()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        StartCoroutine(UpdatePath());
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.5f)
        {
            yield return new WaitForSeconds(0.5f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }

        }
    }

    IEnumerator FollowPath()
    {
        if (path == null)
        {
            yield return null;
        }
        Vector3 currentWaypoint = path[0];
        bool followingPath = true;
        int pathIndex = 0;
        int pathFinishIndex = path.Length-1;

        Quaternion startRotation = Quaternion.LookRotation(path[pathIndex] - transform.position);
        //transform.LookAt(path[0]);
        while (Quaternion.Angle(transform.rotation, startRotation) < 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, Time.deltaTime * turnSpeed);
            yield return null;
        }

        while (followingPath)
        {

            while (Vector3.Distance(transform.position, path[pathIndex]) <= turnDistance)
            {
                if (pathIndex == pathFinishIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation(path[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);


                //if (i == targetIndex)
                //{
                //    Gizmos.DrawLine(transform.position, path[i]);
                //}
                //else
                //{
                //    Gizmos.DrawLine(path[i-1], path[i]);
                //}
            }
        }
    }
}
