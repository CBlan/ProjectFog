using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenMachine : MonoBehaviour {

    public GameObject display;
    public Text displayText;
    public GameObject button;
    public Material buttonMaterial;
    //public TW_Regular TWScript;
    private bool machineActive;
    private float maxDistance = 50;

    public Color normal;
    public Color mouseOver;
    public Color clicked;
    public Color disabled;
    public Color noCredits;
    //private bool TWStarted;

    public float oxygenCost = 100;

    private void Start()
    {
        display.SetActive(false);
    }

    private void Update()
    {
        if (machineActive)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance) && hit.collider.gameObject == button)
            {

                buttonMaterial.SetColor("_EmissionColor", mouseOver * Mathf.PingPong(Time.time * 3, 3));
                displayText.text = "Refill <b>Oxygen</b> tanks\n ---------------\n " + oxygenCost + " Units";
                //if (!TWStarted)
                //{
                //    displayText.text = "Refill <b>Oxygen</b> tanks\n ---------------\n " + oxygenCost + " Units";
                //    TWScript.StartTypewriter();
                //    TWStarted = true;
                //}

                if (Input.GetKey(KeyCode.E))
                {
                    if (oxygenCost <= GameManager.instance.credits)
                    {
                        buttonMaterial.SetColor("_EmissionColor", clicked * 3);
                    }
                    else
                    {
                        buttonMaterial.SetColor("_EmissionColor", noCredits * 3);
                        displayText.text = "Insufficient funds!";
                    }
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
                    if (oxygenCost <= GameManager.instance.credits)
                    {
                        GameManager.instance.oxygen = GameManager.instance.maxOxygen;
                        GameManager.instance.credits -= oxygenCost;

                        displayText.text = "Refill <b>Oxygen</b> tanks\n ---------------\n " + oxygenCost + " Units";
                        //TWStarted = false;
                        //if (!TWStarted)
                        //{
                        //    displayText.text = "Refill <b>Oxygen</b> tanks\n ---------------\n " + oxygenCost + " Units";
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
                displayText.text = "Please Select an item\n <b>↓</b>";
            }

        }
        else
        {
           // buttonMaterial.SetColor("_EmissionColor", disabled);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            display.SetActive(true);
            machineActive = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            display.SetActive(false);
            machineActive = false;
        }
    }
}
