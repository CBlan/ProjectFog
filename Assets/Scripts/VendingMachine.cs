using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VendingMachine : MonoBehaviour {

    public GameObject display;

    private float maxDistance = 500;
    private bool machineActive;

    public GameObject lookingAt;
    public Text displayText;


    private void Start()
    {
        display.SetActive(false);
    }

    private void Update()
    {
        if (machineActive)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance) && hit.collider.gameObject.CompareTag("Upgrade"))
            {
                lookingAt = hit.collider.gameObject;


            }
            else
            {
                lookingAt = null;
                displayText.text = "Please Select an item\n <b>↓      ↓      ↓      ↓</b>";
            }

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
