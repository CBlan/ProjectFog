using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Ranged : MonoBehaviour {

    public float pathUpdateMoveThreshhold = 0.5f;
    public float[] minPathUpdateTime = new float[2] { 0.5f, 2f };

    public Transform player;
    public float speed = 7;

    public float turnDistance = 1;
    public float turnSpeed = 5;

    public float minPatrolDistance = 3;
    private PatrolArea patArea;

    Vector3[] path;
    int targetIndex;
    private Rigidbody rB;

    private float stucktimer;

    bool followingPath;

    private EnemyHealth hP;
    //private Vector3 pathCheck;

    private void Start()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        GameManager.instance.enemies.Add(gameObject);
        GameManager.instance.sM.currentRanged++;
        player = GameManager.instance.player.transform;
        rB = GetComponent<Rigidbody>();
        patArea = GameManager.instance.rangedPatArea;
        StartCoroutine(UpdatePath());
        StartCoroutine(CheckIfStuck());
        hP = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            transform.LookAt(player);
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
        }
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
        Vector3 newPatPoint = Vector3.zero;
        //PathRequestManager.RequestPath(new PathRequest(transform.position, newPatPoint, OnPathFound));

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = player.position;
        Vector3 unitPosOld = transform.position;

        while (true)
        {

            yield return new WaitForSeconds(Random.Range(minPathUpdateTime[0], minPathUpdateTime[1]));
            if (Vector3.Distance(transform.position, newPatPoint) < minPatrolDistance || newPatPoint == Vector3.zero)
            {
                //print("here");
                newPatPoint = patArea.GetPatrolPoint();
                PathRequestManager.RequestPath(new PathRequest(transform.position, newPatPoint, OnPathFound));
                targetPosOld = newPatPoint;
                //pathCheck = newPatPoint;
            }
            //print(Vector3.Distance(transform.position, newPatPoint) + " < " + minPatrolDistance);
            yield return null;
        }
    }

    IEnumerator FollowPath()
    {
        followingPath = true;
        int pathIndex = 0;
        int pathFinishIndex = path.Length - 1;

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
                while (Quaternion.Angle(transform.rotation, targetRotation) > 50)
                {
                    //print(Quaternion.Angle(transform.rotation, targetRotation) + " > 50");
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                    yield return null;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
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
            yield return new WaitForSeconds(3f);
            if (Vector3.Distance(checkPos, transform.position) < 0.1f)
            {
                hP.CreditsValue = 0;
                hP.DestroySelf();
            }
            yield return null;
        }
    }

}