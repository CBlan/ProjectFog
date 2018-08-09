using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float bulletDamage = 10;

    private void Start()
    {
        //print("spawned");
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.instance.player)
        {
            GameManager.instance.DamagePlayer(bulletDamage);
        }
        Destroy(gameObject);
    }
}
