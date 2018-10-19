using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Grenades : MonoBehaviour {

    private bool buttonPressed;
    public GameObject crosshair;
    public GameObject bar1;
    public GameObject bar2;


    private void Start()
    {
        crosshair.SetActive(true);
        bar1.SetActive(true);
        bar2.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1") && !buttonPressed)
        {
            Tutorial.controller.NextPannel();
            buttonPressed = true;
        }
    }
}
