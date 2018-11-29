using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackDrop : MonoBehaviour {

    public GameObject particles;
    public float upgradeAmmount = 50;
    public float regenUpAmmount = 0.25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.playerScript.maxJetpackFuel += upgradeAmmount;
            GameManager.instance.playerScript.jetFuelRegenRate += upgradeAmmount;
            GameManager.instance.oxygen = GameManager.instance.maxOxygen;
            Instantiate(particles, transform.position, transform.rotation);
            Fabric.EventManager.Instance.PostEvent("Player/Teleport/Lvl1", Fabric.EventAction.PlaySound, gameObject);
            Destroy(gameObject);
        }
    }

}
