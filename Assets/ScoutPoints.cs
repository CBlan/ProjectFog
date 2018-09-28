using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutPoints : MonoBehaviour {

    public List<Transform> patrolPoints;

    void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            patrolPoints.Add(trans);

        }
    }

    public Vector3 GetPatrolPoint()
    {
        foreach (Transform point in patrolPoints)
        {
            if (!Physics.CheckSphere(point.position, 0.5f))
            {
                if (CheckLOS(point.position, GameManager.instance.player, 10))
                {
                    patrolPoints.Remove(point);
                    patrolPoints.Add(point);
                    return point.position;

                }
            }
        }
        return Vector3.zero;

        //Vector3 patPoint = patrolPoints[Random.Range(0, patrolPoints.Length)].position;

        //if (!Physics.CheckSphere(patPoint, 0.5f))
        //{
        //    if (CheckLOS(patPoint, GameManager.instance.player, 10))
        //    {
        //        return patPoint;

        //    }
        //}
        //return Vector3.zero;

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
