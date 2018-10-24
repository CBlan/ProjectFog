using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float maxHP = 100;
    public float health;
    public float CreditsValue;
    public int enemyKilled = 1;
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
        if (gameObject.CompareTag("Melee") && GameManager.instance.sM != null)
        {
            GameManager.instance.sM.currentMelee--;
        }
        if (gameObject.CompareTag("Ranged") && GameManager.instance.sM != null)
        {
            GameManager.instance.sM.currentRanged--;
        }
        if (gameObject.CompareTag("Scout") && GameManager.instance.sM != null)
        {
            GameManager.instance.sM.currentScout--;
        }
        Instantiate(destroyEffect, transform.position, transform.rotation);
        GameManager.instance.credits += CreditsValue;
        ScoreCollector.score.enemiesKilled += enemyKilled;
        Destroy(gameObject);
    }
}
