using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolArea_Ranged : MonoBehaviour {

    public Transform[] patrolPoints;
    //public Vector3 gridWorldSize;
    //public float walkableCheckRadius = 0.4f;

    //public bool visualiseVolume = true;

    //private float gridXTopWorld, gridXBotWorld, gridYTopWorld, gridYBotWorld, gridZTopWorld, gridZBotWorld;

    // Use this for initialization
    void Start () {
        //gridXTopWorld = transform.position.x + (gridWorldSize.x / 2);
        //gridXBotWorld = transform.position.x - (gridWorldSize.x / 2);
        //gridYTopWorld = transform.position.y + (gridWorldSize.y / 2);
        //gridYBotWorld = transform.position.y - (gridWorldSize.y / 2);
        //gridZTopWorld = transform.position.z + (gridWorldSize.z / 2);
        //gridZBotWorld = transform.position.z - (gridWorldSize.z / 2);
        patrolPoints = new Transform[transform.childCount]; 

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform trans = transform.GetChild(i);
            patrolPoints[i] = trans;

        }
    }

    //private void OnDrawGizmos()
    //{
    //if (visualiseVolume)
    //{
    //    Gizmos.color = Color.blue;

    //   Gizmos.DrawWireCube(transform.position, gridWorldSize);
    //}


    //}

    public Vector3 GetPatrolPoint(Vector3 position)
    {
        Vector3 patPoint;
        Collider[] hitColliders = Physics.OverlapSphere(position, 20, 1 << LayerMask.NameToLayer("RangedPatPoints"));
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
