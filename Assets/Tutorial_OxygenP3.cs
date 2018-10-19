using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_OxygenP3 : MonoBehaviour {

    private bool itemPurchased;
    private bool buttonPressed;

    public GameObject vendor;
    private GameObject vend;

    private void Start()
    {
        GameManager.instance.credits = 100;
        vend = Instantiate(vendor, Tutorial.controller.player.transform.position, Tutorial.controller.player.transform.rotation);
        //vend.transform.parent = this.transform;
    }

    void Update()
    {
        if (GameManager.instance.oxygen == 100 && !buttonPressed)
        {
            Tutorial.controller.NextPannel();
            buttonPressed = true;
        }
    }
}
