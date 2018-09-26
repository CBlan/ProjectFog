using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutPoints : MonoBehaviour {

    public Transform[] patrolPoints;

    void Start()
    {

        patrolPoints = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            patrolPoints[i] = trans;

        }
    }

    public Vector3 GetPatrolPoint()
    {
        Vector3 patPoint = patrolPoints[Random.Range(0, patrolPoints.Length)].position;

        if (!Physics.CheckSphere(patPoint, 0.5f))
        {
            if (CheckLOS(patPoint, GameManager.instance.player, 10))
            {
                return patPoint;

            }
        }
        return Vector3.zero;

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
}
