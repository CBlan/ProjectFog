using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_CreditsP2 : MonoBehaviour {

    private bool itemPurchased;
    private bool buttonPressed;

    public GameObject vendor;
    private GameObject vend;

    private void Start()
    {
        vend = Instantiate(vendor, Tutorial.controller.player.transform.position, Tutorial.controller.player.transform.rotation);
        //vend.transform.parent = this.transform;
    }

    void Update()
    {
        if (GameManager.instance.credits == 0 && !buttonPressed)
        {
            Tutorial.controller.NextPannel();
            buttonPressed = true;
        }
    }
}
