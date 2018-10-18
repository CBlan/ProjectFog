using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Movment : MonoBehaviour {

    private bool buttonPressed;

    public GameObject indicator;

    private void Start()
    {
        Instantiate(indicator, Tutorial.controller.player.transform.position, Tutorial.controller.player.transform.rotation);
    }

    void Update () {
        if (Input.GetAxis("Horizontal") != 0 && !buttonPressed || Input.GetAxis("Vertical") != 0 && !buttonPressed)
        {
            Tutorial.controller.NextPannel();
            buttonPressed = true;
        }
    }
}
