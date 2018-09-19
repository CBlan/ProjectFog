using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeJetpack : MonoBehaviour {

    public VendingMachine vendingScript;
    public Material buttonMaterial;
    public float upgradeAmmount = 10;
    public float upgradeCost = 100;
    public Color normal;
    public Color mouseOver;
    public Color clicked;
    public Color disabled;
    public Color noCredits;
    public Text display;
    public TW_Regular TWScript;
    private bool TWStarted;
    private int upgradesUsed;


    void Update()
    {
        if (vendingScript.display.activeSelf)
        {
            if (vendingScript.lookingAt == this.gameObject)
            {

                buttonMaterial.SetColor("_EmissionColor", mouseOver * Mathf.PingPong(Time.time * 3, 3));
                if (!TWStarted)
                {
                    display.text = "Upgrade <b>Jetpack</b> capacity\n ---------------\n " + upgradeCost + " Units";
                    TWScript.StartTypewriter();
                    TWStarted = true;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    if (upgradeCost <= GameManager.instance.credits)
                    {
                        buttonMaterial.SetColor("_EmissionColor", clicked * 3);
                    }
                    else
                    {
                        buttonMaterial.SetColor("_EmissionColor", noCredits * 3);
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
                            buttonMaterial.SetColor("_EmissionColor", disabled);
                            this.enabled = false;
                        }

                        TWStarted = false;
                        if (!TWStarted)
                        {
                            display.text = "Upgrade <b>Jetpack</b> capacity\n ---------------\n " + upgradeCost + " Units";
                            TWScript.StartTypewriter();
                            TWStarted = true;
                        }
                    }
                }
            }
            else
            {
                buttonMaterial.SetColor("_EmissionColor", normal * Mathf.PingPong(Time.time * 3, 3));
                TWStarted = false;
            }
        }
        else buttonMaterial.SetColor("_EmissionColor", disabled);
    }
}
