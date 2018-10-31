using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Oxygen : MonoBehaviour {

    public GameObject bar;

	// Use this for initialization
	void Start () {
        Destroy(GameObject.FindGameObjectWithTag("Tutorial"));
        GameManager.instance.oxygenDegenRate = 0.1f;
        bar.SetActive(true);
        Tutorial.controller.NextPannel();
    }
	
}
