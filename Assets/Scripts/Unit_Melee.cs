using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Melee : MonoBehaviour {

    public float pathUpdateMoveThreshhold = 0.5f;
    public float[] minPathUpdateTime = new float[2] {0.3f, 0.7f};

    public Transform player;
    public float speed = 7;

    public float turnDistance = 1;
    public float turnSpeed = 5;

    public float minPatrolDistance = 3;
    private PatrolArea_Melee patArea;

    public float alertDistance = 5;
    public float sightRange = 20;
    public float fieldOfView = 60;
    private float fieldOfViewRangeInHalf;

    Vector3[] path;
    int targetIndex;
    private Rigidbody rB;

    private float stucktimer;

    private AlertStatus alertStatus;
    bool followingPath;

    private EnemyHealth hP;
    //private Vector3 pathCheck;

    private void OnEnable()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        //GameManager.instance.enemies.Add(gameObject);
        GameManager.instance.sM.currentMelee++;
        fieldOfViewRangeInHalf = fieldOfView / 2;
        player = GameManager.instance.player.transform;
        alertStatus = GetComponent<AlertStatus>();
        rB = GetComponent<Rigidbody>();
        patArea = GameManager.instance.meleePatArea;
        StartCoroutine(UpdatePath());
        StartCoroutine(CheckIfStuck());
        StartCoroutine(CheckIfAlerted());
        hP = GetComponent<EnemyHealth>();
        //Fabric.EventManager.Instance.PostEvent("Enemy/Melee/Unit", Fabric.EventAction.PlaySound, gameObject);
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

    IEnumerator CheckIfAlerted()
    {
        if (Time.timeSinceLevelLoad < 0.5f)
        {
            yield return new WaitForSeconds(0.5f);
        }

        while (true)
        {
            //check if player is within short range
            if (!alertStatus.alerted && (Vector3.Distance(player.position, transform.position) < alertDistance))
            {
                alertStatus.SetAlert();
            }

            //check if player is within sight
            if (!alertStatus.alerted && CanSeePlayer())
            {
                alertStatus.SetAlert();
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
        Vector3 newPatPoint = Vector3.zero;
        //PathRequestManager.RequestPath(new PathRequest(transform.position, newPatPoint, OnPathFound));

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = player.position;
        Vector3 unitPosOld = transform.position;

        while (true) {

            while (alertStatus.alerted)
            {
                //yield return new WaitForSeconds(Random.Range(minPathUpdateTime[0], minPathUpdateTime[1]));
                if ((player.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
                {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, player.position, OnPathFound));
                    targetPosOld = player.position;
                }

                else if ((transform.position - unitPosOld).sqrMagnitude > sqrMoveThreshhold)
                {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, player.position, OnPathFound));
                    unitPosOld = transform.position;
                }

                yield return new WaitForSeconds(Random.Range(minPathUpdateTime[0], minPathUpdateTime[1]));
            }

            while (!alertStatus.alerted)
            {

                yield return new WaitForSeconds(Random.Range(minPathUpdateTime[0], minPathUpdateTime[1]));
                if (Vector3.Distance(transform.position, newPatPoint) < minPatrolDistance || newPatPoint == Vector3.zero)
                {
                    //print("here");
                    newPatPoint = patArea.GetPatrolPoint(transform.position);
                    PathRequestManager.RequestPath(new PathRequest(transform.position, newPatPoint, OnPathFound));
                    targetPosOld = newPatPoint;
                    //pathCheck = newPatPoint;
                }
                //print(Vector3.Distance(transform.position, newPatPoint) + " < " + minPatrolDistance);
            }
            yield return null;
        }
    }

    IEnumerator FollowPath()
    {
        followingPath = true;
        int pathIndex = 0;
        int pathFinishIndex = path.Length-1;

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
                hP.enemyKilled = 0;
                hP.CreditsValue = 0;
                hP.DestroySelf();
            }
            yield return null;
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 rayDir = player.transform.position - transform.position;

        if ((Vector3.Angle(rayDir, transform.forward)) < fieldOfViewRangeInHalf)
        {
            if (Physics.Raycast(transform.position, rayDir, out hit, sightRange))
            {

                if (hit.collider.gameObject == player.gameObject)
                {
                    //Debug.Log("Can see player");
                    return true;
                }
                else
                {
                    //Debug.Log("Can not see player");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void OnDisable()
    {
        //Fabric.EventManager.Instance.PostEvent("Enemy/Melee/Unit", Fabric.EventAction.StopSound, gameObject);
        StopCoroutine(UpdatePath());
        StopCoroutine(CheckIfStuck());
        StopCoroutine(FollowPath());
        StopCoroutine(CheckIfAlerted());
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

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawCube(pathCheck, Vector3.one);
    //}
}
