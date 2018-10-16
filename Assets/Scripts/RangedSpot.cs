using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpot : MonoBehaviour {

    private GameObject spotLight;
    private Light lightComp;
    private Unit_RangedOLD unitScript;

    // Use this for initialization
    void Start () {
        unitScript = GetComponent<Unit_RangedOLD>();
        spotLight = new GameObject("Spot Light");
        lightComp = spotLight.AddComponent<Light>();
        lightComp.type = LightType.Spot;
        lightComp.color = Color.red;
        lightComp.spotAngle = unitScript.fieldOfView;
        lightComp.range = unitScript.sightRange;
        lightComp.intensity = 2;
        spotLight.transform.rotation = Quaternion.Euler(90,0,0);

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        spotLight.transform.position = transform.position;
    }

    private void OnDestroy()
    {
        Destroy(spotLight);
    }
}
