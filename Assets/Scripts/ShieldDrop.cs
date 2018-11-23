using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDrop : MonoBehaviour {

    public GameObject particles;
    public float upgradeAmmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.maxPlayerHP += upgradeAmmount;
            GameManager.instance.regenRate += upgradeAmmount / 50;
            GameManager.instance.oxygen = GameManager.instance.maxOxygen;
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
