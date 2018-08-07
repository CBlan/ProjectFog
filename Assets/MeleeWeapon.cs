using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour {

    public float damage = 1;
    public float damageInterval = 0.5f;
    private float cooldown;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (cooldown > damageInterval)
            {
                GameManager.instance.playerHP -= damage;
                cooldown = 0;
            }
            cooldown += Time.deltaTime; 
        }
    }
}
