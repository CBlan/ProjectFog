﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeJetpack : MonoBehaviour {

    public VendingMachine vendingScript;
    Image upgradeImage;
    public float upgradeAmmount = 10;
    public float upgradeCost = 100;
    public Color normal;
    public Color mouseOver;
    public Color clicked;
    public Color disabled;
    public Color noCredits;
    public Text display;
    private int upgradesUsed;
    // Use this for initialization
    void Start()
    {
        upgradeImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vendingScript.lookingAt == this.gameObject)
        {
            upgradeImage.color = mouseOver;
            display.text = "Increases jetpack capacity - " + upgradeCost;

            if (Input.GetKey(KeyCode.E))
            {
                if (upgradeCost <= GameManager.instance.credits)
                {
                    upgradeImage.color = clicked;
                }
                else
                {
                    upgradeImage.color = noCredits;
                    display.text = "Insufficient funds!";
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (upgradeCost <= GameManager.instance.credits)
                {
                    GameManager.instance.playerScript.maxJetpackFuel += upgradeAmmount;
                    //GameManager.instance.playerScript.jetFuelRegenRate += upgradeAmmount/30;
                    GameManager.instance.credits -= upgradeCost;
                    upgradesUsed++;
                    upgradeCost += upgradeCost;
                    if (upgradesUsed == 4)
                    {
                        upgradeImage.color = disabled;
                        this.enabled = false;
                    }
                }
            }
        }
        else upgradeImage.color = normal;
    }
}
