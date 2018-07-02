using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolArea : MonoBehaviour {

    public Vector3 gridWorldSize;

    public bool visualiseVolume;

    // Use this for initialization
    void Start () {

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
        return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
    }
}
