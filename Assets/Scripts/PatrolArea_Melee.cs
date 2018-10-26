using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolArea_Melee : MonoBehaviour {

    public Transform[] patrolPoints;

    void Start () {

        patrolPoints = new Transform[transform.childCount]; 

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            patrolPoints[i] = trans;

        }
    }

    public Vector3 GetPatrolPoint(Vector3 position)
    {
        Vector3 patPoint;
        Collider[] hitColliders = Physics.OverlapSphere(position, 30, 1 << LayerMask.NameToLayer("MeleePatPoints"));
        if (hitColliders.Length > 0)
        {
            patPoint = hitColliders[Random.Range(0, hitColliders.Length)].transform.position;
        }
        else
        {
            patPoint = patrolPoints[Random.Range(0, patrolPoints.Length)].position;
        }


        return patPoint;
    }
}
