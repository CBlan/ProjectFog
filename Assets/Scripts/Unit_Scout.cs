using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Scout : MonoBehaviour {

    public float pathUpdateMoveThreshhold = 0.5f;
    public float[] minPathUpdateTime = new float[2] {0.3f, 0.1f};

    public Transform target;
    public float speed = 7;

    public float turnDistance = 1;
    public float turnSpeed = 5;

    Vector3[] path;
    int targetIndex;
    private Rigidbody rB;

    private float stucktimer;

    private void Start()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        rB = GetComponent<Rigidbody>();
        StartCoroutine(UpdatePath());
        StartCoroutine(CheckIfStuck());
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
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPathUpdateTime[0], minPathUpdateTime[1]));
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }

        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        int pathFinishIndex = path.Length-1;

        Quaternion startRotation = Quaternion.LookRotation(path[pathIndex] - transform.position);
        //transform.LookAt(path[0]);
        if (Quaternion.Angle(transform.rotation, startRotation) < 1 && followingPath)
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
                //rB.AddForce(transform.forward * speed, ForceMode.VelocityChange);
            }
            yield return null;
        }
    }

    IEnumerator CheckIfStuck()
    {
        Vector3 checkPos;
        while (true)
        {
            checkPos = transform.position;
            yield return new WaitForSeconds(2f);
            if (Vector3.Distance(checkPos, transform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
            yield return null;
        }
    }

    //public void OnDrawGizmos()
    //{
    //    if (path != null)
    //    {
    //        for (int i = targetIndex; i < path.Length; i++)
    //        {
    //            Gizmos.color = Color.black;
    //            Gizmos.DrawCube(path[i], Vector3.one);


    //            //if (i == targetIndex)
    //            //{
    //            //    Gizmos.DrawLine(transform.position, path[i]);
    //            //}
    //            //else
    //            //{
    //            //    Gizmos.DrawLine(path[i-1], path[i]);
    //            //}
    //        }
    //    }
    //}
}
