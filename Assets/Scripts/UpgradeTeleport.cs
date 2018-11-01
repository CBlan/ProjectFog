using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTeleport : MonoBehaviour {

    public VendingMachine vendingScript;
    public Material buttonMaterial;
    public float upgradeAmmount = 1;
    public float upgradeCost = 100;
    public Color normal;
    public Color mouseOver;
    public Color clicked;
    public Color disabled;
    public Color noCredits;
    public Text display;
    //public TW_Regular TWScript;
    //private bool TWStarted;
    private int upgradesUsed;


    // Update is called once per frame
    void Update()
    {
        if (vendingScript.display.activeSelf)
        {
            if (vendingScript.lookingAt == this.gameObject)
            {
                
                buttonMaterial.SetColor("_EmissionColor", mouseOver * Mathf.PingPong(Time.time * 3, 3));
                display.text = "Upgrade <b>Dash</b> cooldown\n ---------------\n" + upgradeCost + " Units";
                //if (!TWStarted)
                //{
                //    display.text = "Upgrade <b>Teleport</b> cooldown\n ---------------\n" + upgradeCost + " Units";
                //    TWScript.StartTypewriter();
                //    TWStarted = true;
                //}

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
                        GameManager.instance.playerScript.dashCooldown -= upgradeAmmount;
                        GameManager.instance.credits -= upgradeCost;
                        upgradesUsed++;
                        upgradeCost += upgradeCost;
                        if (upgradesUsed == 4)
                        {
                            buttonMaterial.SetColor("_EmissionColor", disabled);
                            this.enabled = false;
                        }
                        display.text = "Upgrade <b>Dash</b> cooldown\n ---------------\n" + upgradeCost + " Units";
                        //TWStarted = false;
                        //if (!TWStarted)
                        //{
                        //    display.text = "Upgrade <b>Teleport</b> cooldown\n ---------------\n" + upgradeCost + " Units";
                        //    TWScript.StartTypewriter();
                        //    TWStarted = true;
                        //}
                    }
                }
            }
            else
            {
                buttonMaterial.SetColor("_EmissionColor", normal * Mathf.PingPong(Time.time * 3, 3));
                //TWStarted = false;
            }
        }
        //else buttonMaterial.SetColor("_EmissionColor", disabled);
    }
}
