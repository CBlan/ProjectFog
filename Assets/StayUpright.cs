using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayUpright : MonoBehaviour {

    public Transform parent;
    Vector3 desiredRotation;

	// Use this for initialization
	void Start () {
        parent = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
        //desiredRotation.x = -parent.rotation.eulerAngles.x;
        //desiredRotation.y = parent.rotation.eulerAngles.y;
        //desiredRotation.z = -parent.rotation.eulerAngles.z;

        //transform.rotation = Quaternion.Euler(desiredRotation);
        var projection = Vector3.ProjectOnPlane(parent.transform.forward, Vector3.up);

        var angle = Vector3.Angle(transform.forward, projection);

        Debug.DrawRay(transform.position, projection, Color.green);
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        transform.rotation = Quaternion.LookRotation(projection);
    }
}
