using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Health : MonoBehaviour {

    public GameObject HealthBar1;
    public GameObject HealthBar2;
    // Use this for initialization
    void Start () {
        HealthBar1.SetActive(true);
        HealthBar2.SetActive(true);
        Tutorial.controller.NextPannel();
    }

}
