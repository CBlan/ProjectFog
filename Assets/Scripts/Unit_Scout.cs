using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Scout : MonoBehaviour {

    public float pathUpdateMoveThreshhold = 0.5f;
    public float[] minPathUpdateTime = new float[2] {0.3f, 0.7f};

    private Transform target;
    private Transform player;
    public float speed = 7;

    public float turnDistance = 1;
    public float turnSpeed = 5;
    public float sightRange = 5;

    Vector3[] path;
    int targetIndex;
    private Rigidbody rB;

    private float stucktimer;

    private void Start()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        GameManager.instance.enemies.Add(gameObject);
        rB = GetComponent<Rigidbody>();
        player = GameManager.instance.player.transform;
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

    private void Update()
    {

        if (Vector3.Distance(transform.position, player.position) < sightRange + 0.5f)
        {
            RaycastHit hit;
            Vector3 rayDir = player.position - transform.position;

            if (Physics.Raycast(transform.position, rayDir, out hit, sightRange))
            {
                if (hit.collider.gameObject.transform == player)
                {
                    GameObject closestUnalert;
                    closestUnalert = FindClosestUnalertEnemy();

                    if (closestUnalert != null)
                    {
                        closestUnalert.GetComponent<AlertStatus>().alerted = true;
                    }
                }
            }
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.5f)
        {
            yield return new WaitForSeconds(0.5f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, player.position, OnPathFound));

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = player.position;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPathUpdateTime[0], minPathUpdateTime[1]));
            if ((player.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, player.position, OnPathFound));
                targetPosOld = player.position;
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

    public GameObject FindClosestUnalertEnemy()
    {
        List<GameObject> nearestEnemies = new List<GameObject> (GameManager.instance.enemies);
        GameObject closest = null;
        AlertStatus aS;
        float distance = Mathf.Infinity;
        foreach (GameObject enemy in nearestEnemies)
        {;
            if (aS  = enemy.GetComponent<AlertStatus>())
            {
                if (!aS.alerted)
                {
                    Vector3 diff = enemy.transform.position - player.position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = enemy;
                        distance = curDistance;
                    }
                }
            }
        }
        return closest;
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
