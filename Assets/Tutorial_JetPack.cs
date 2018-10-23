using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_JetPack : MonoBehaviour {

    private bool buttonPressed;

    public GameObject indicator;
    private GameObject indi;

    private PlayerMoveV3 playerScript;

    private void Start()
    {
        indi = Instantiate(indicator, Tutorial.controller.player.transform.position, Tutorial.controller.player.transform.rotation);
        indi.transform.parent = this.transform;
        playerScript = GameManager.instance.player.GetComponent<PlayerMoveV3>();
        playerScript.maxJetpackFuel = 100;
        playerScript.jetFuel = 100;
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
