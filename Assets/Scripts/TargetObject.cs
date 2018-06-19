using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour {

    public static TargetObject tarObj;
    public GameObject target;
    public Vector3 hitPoint;


	// Use this for initialization
	void Start () {
        tarObj = this;

	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;

        var cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));


        if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 1000))
        {
            target = hit.transform.gameObject;
            hitPoint = hit.point;
        }
        else
        {
            target = null;
            hitPoint = Camera.main.ViewportToWorldPoint( new Vector3(0.5f, 0.5f, 100.0f));
        }
    }
}
