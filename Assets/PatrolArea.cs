using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolArea : MonoBehaviour {

    public Vector3 gridWorldSize;
    public float walkableCheckRadius = 0.4f;

    public bool visualiseVolume = true;

    private float gridXTopWorld, gridXBotWorld, gridYTopWorld, gridYBotWorld, gridZTopWorld, gridZBotWorld;

    // Use this for initialization
    void Start () {
        gridXTopWorld = transform.position.x + (gridWorldSize.x / 2);
        gridXBotWorld = transform.position.x - (gridWorldSize.x / 2);
        gridYTopWorld = transform.position.y + (gridWorldSize.y / 2);
        gridYBotWorld = transform.position.y - (gridWorldSize.y / 2);
        gridZTopWorld = transform.position.z + (gridWorldSize.z / 2);
        gridZBotWorld = transform.position.z - (gridWorldSize.z / 2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        if (visualiseVolume)
        {
            Gizmos.color = Color.blue;

           Gizmos.DrawWireCube(transform.position, gridWorldSize);
        }


    }

    public Vector3 GetPatrolPoint()
    {
        Vector3 patPoint = Vector3.zero;
        bool isWalkable = false;

        while (!isWalkable)
        {
            patPoint = new Vector3(Random.Range(gridXBotWorld, gridXTopWorld), Random.Range(gridYBotWorld, gridYTopWorld), Random.Range(gridZBotWorld, gridZTopWorld));
            isWalkable = !(Physics.CheckSphere(patPoint, walkableCheckRadius));
        }


        return patPoint;
    }
}
