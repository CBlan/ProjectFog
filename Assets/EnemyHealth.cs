using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float maxHP = 100;
    public float health;
    public GameObject destroyEffect;

    private void Start()
    {
        health = maxHP;
    }

    private void Update()
    {
        if (health <= 0)
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        GameManager.instance.enemies.Remove(gameObject);
        Instantiate(destroyEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
