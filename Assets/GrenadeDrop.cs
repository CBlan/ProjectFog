using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeDrop : MonoBehaviour {

    public GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.grenadeThrowerScript.currentGrenade += 1;
            GameManager.instance.grenadeThrowerScript.cooldown -= 0.3f;
            GameManager.instance.oxygen = GameManager.instance.maxOxygen;
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
