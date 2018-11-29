using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDrop : MonoBehaviour {

    public GameObject particles;
    public float upgradeAmmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.playerScript.dashCooldown -= upgradeAmmount;
            GameManager.instance.oxygen = GameManager.instance.maxOxygen;
            Instantiate(particles, transform.position, transform.rotation);
            Fabric.EventManager.Instance.PostEvent("Player/Teleport/Lvl1", Fabric.EventAction.PlaySound, gameObject);
            Destroy(gameObject);
        }
    }
}
