using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_DashP2 : MonoBehaviour {

    public GameObject bar1;
    public GameObject bar2;
    // Use this for initialization
    void Start()
    {
        bar1.SetActive(true);
        bar2.SetActive(true);
        Tutorial.controller.NextPannel();
    }
}
