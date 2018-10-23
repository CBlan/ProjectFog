using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Dash : MonoBehaviour {

    private bool buttonPressed;

    public GameObject indicator;
    private GameObject indi;

    private void Start()
    {
        Destroy(GameObject.FindGameObjectWithTag("Tutorial"));
        indi = Instantiate(indicator, Tutorial.controller.player.transform.position, Tutorial.controller.player.transform.rotation);
        indi.transform.parent = this.transform;
    }

    void Update()
    {
        if (Input.GetButtonDown("Run") && !buttonPressed)
        {
            Tutorial.controller.NextPannel();
            buttonPressed = true;
        }
    }
}
