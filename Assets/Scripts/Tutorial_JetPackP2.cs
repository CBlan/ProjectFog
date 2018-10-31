using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_JetPackP2 : MonoBehaviour {

    public GameObject bar;
    // Use this for initialization
    void Start()
    {
        bar.SetActive(true);
        Tutorial.controller.NextPannel();
    }
}
