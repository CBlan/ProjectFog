using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour {

    public GameObject dispaly;
    private float maxDistance = 500;
    private bool machineActive;

    private void Start()
    {
        dispaly.SetActive(false);
    }

    private void Update()
    {
        if (machineActive)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance) && hit.collider.gameObject.CompareTag("Upgrade"))
            {



            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dispaly.SetActive(true);
            machineActive = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dispaly.SetActive(false);
            machineActive = false;
        }
    }
}
