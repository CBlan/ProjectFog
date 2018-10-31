using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Credits : MonoBehaviour {

    public GameObject credits;
    public GameObject hUD;
    // Use this for initialization
    void Start()
    {
        credits.SetActive(true);
        hUD.SetActive(true);
        Tutorial.controller.NextPannel();
    }
}
