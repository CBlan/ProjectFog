using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Scout : MonoBehaviour {

    public float pathUpdateMoveThreshhold = 0.5f;
    public float[] minPathUpdateTime = new float[2] {0.3f, 0.7f};

    private Vector3 target;
    private GameObject player;
    public float speed = 7;

    public float turnDistance = 1;
    public float turnSpeed = 5;
    public float sightRange = 10;
    public float alertingEnemyTime = 4f;

    Vector3[] path;
    int targetIndex;
    private Rigidbody rB;

    private float stucktimer;

    private EnemyHealth hP;
    private ScoutPoints patArea;

    private void Start()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        GameManager.instance.enemies.Add(gameObject);
        GameManager.instance.sM.currentScout++;
        rB = GetComponent<Rigidbody>();
        hP = GetComponent<EnemyHealth>();
        player = GameManager.instance.player;
        patArea = GameManager.instance.scoutPoints;
        target = Vector3.zero;
        StartCoroutine(UpdatePath());
        StartCoroutine(CheckIfStuck());
        //StartCoroutine(FindTarget());
        StartCoroutine(AlertEnemy());
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

    //private void Update()
    //{

    //    if (Vector3.Distance(transform.position, player.transform.position) < sightRange + 0.5f)
    //    {

    //        if (CheckLOS(transform.position, player, sightRange))
    //        {
    //            GameObject closestUnalert;
    //            closestUnalert = FindClosestUnalertEnemy();

    //            if (closestUnalert != null)
    //            {
    //                closestUnalert.GetComponent<AlertStatus>().alerted = true;
    //            }
    //        }
    //    }
    //}

    IEnumerator AlertEnemy()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightRange + 0.5f)
            {

                if (CheckLOS(transform.position, player, sightRange))
                {
                    GameObject closestUnalert;
                    closestUnalert = FindClosestUnalertEnemy();

                    if (closestUnalert != null)
                    {
                        closestUnalert.GetComponent<AlertStatus>().SetAlert();
                        yield return new WaitForSeconds(alertingEnemyTime);
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.5f)
        {
            yield return new WaitForSeconds(0.5f);
        }
        //PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = target;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPathUpdateTime[0], minPathUpdateTime[1]));
            if (Vector3.Distance(transform.position, target) < 10 || target == Vector3.zero)
            {
                target = patArea.GetPatrolPoint();
                PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
                print(target);
                targetPosOld = target;
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
            yield return new WaitForSeconds(5f);
            if (Vector3.Distance(checkPos, transform.position) < 0.1f)
            {
                hP.DestroySelf();
            }
            yield return null;
        }
    }

    //IEnumerator FindTarget()
    //{
    //    while (true)
    //    {
    //        Vector3 targetPosition = player.transform.position + (Random.onUnitSphere * (sightRange - 2));
    //        //targetPosition.y = Mathf.Abs(targetPosition.y);

    //        if (!Physics.CheckSphere(targetPosition, 0.5f))
    //        {
    //            if (CheckLOS(targetPosition, player, sightRange))
    //            {
    //                target = targetPosition;
    //                //print("target");
    //                yield return new WaitForSeconds(0.5f);
    //            }
    //        }
    //        //print("no target");
    //        yield return null;
    //    }
    //}

    public GameObject FindClosestUnalertEnemy()
    {
        //List<GameObject> nearestEnemies = new List<GameObject> (GameManager.instance.enemies);
        GameObject closest = null;
        float distance = 10000;
        foreach (GameObject enemy in GameManager.instance.enemies)
        {

            if (enemy.layer == LayerMask.NameToLayer("Enemy"))
            {

                Vector3 diff = enemy.transform.position - player.transform.position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = enemy;
                    distance = curDistance;
                }

            }
        }
        return closest;
    }

    bool CheckLOS(Vector3 position, GameObject target, float sightRange)
    {
        RaycastHit hit;
        Vector3 rayDir = target.transform.position - position;

        if (Physics.Raycast(position, rayDir, out hit, sightRange))
        {
            if (hit.collider.gameObject == target)
            {
                return true;
            }
            else return false;
        }
        return false;
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
