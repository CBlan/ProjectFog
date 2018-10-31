using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Movment : MonoBehaviour {

    private bool buttonPressed;

    public GameObject indicator;
    private GameObject indi;

    private void Start()
    {
        indi = Instantiate(indicator, Tutorial.controller.player.transform.position, Tutorial.controller.player.transform.rotation);
        indi.transform.parent = this.transform;
    }

    void Update () {
        if (Input.GetAxis("Horizontal") != 0 && !buttonPressed || Input.GetAxis("Vertical") != 0 && !buttonPressed)
        {
            Tutorial.controller.NextPannel();
            buttonPressed = true;
        }
    }
}
