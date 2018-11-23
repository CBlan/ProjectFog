using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackDrop : MonoBehaviour {

    public GameObject particles;
    public float upgradeAmmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.playerScript.maxJetpackFuel += upgradeAmmount;
            GameManager.instance.oxygen = GameManager.instance.maxOxygen;
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
