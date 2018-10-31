using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Jump : MonoBehaviour
{

    private bool buttonPressed;

    public GameObject indicator;
    private GameObject indi;

    private void Start()
    {
        indi = Instantiate(indicator, Tutorial.controller.player.transform.position, Tutorial.controller.player.transform.rotation);
        indi.transform.parent = this.transform;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !buttonPressed)
        {
            Tutorial.controller.NextPannel();
            buttonPressed = true;
        }
    }
}
