using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenDrop : MonoBehaviour {

    public GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.oxygen = GameManager.instance.maxOxygen;
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
