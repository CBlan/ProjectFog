using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float maxHP = 100;
    public float health;
    public float CreditsValue;
    private float creditsValueStart;
    public int enemyKilled = 1;
    public GameObject destroyEffect;
    private GameObject droppedItem;

    private void Start()
    {
        health = maxHP;
        creditsValueStart = CreditsValue;
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
        //GameManager.instance.enemies.Remove(gameObject);
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
        Fabric.EventManager.Instance.PostEvent("Enemy/Explosion", Fabric.EventAction.PlaySound, gameObject);
        GameManager.instance.credits += CreditsValue;
        GameManager.instance.enemiesKilled += enemyKilled;
        if (GameManager.instance.enemiesKilled !=0 && GameManager.instance.enemiesKilled % 5 == 0)
        {
            if (GameManager.instance.upgradePickups.Count > 0)
            {
                droppedItem = Instantiate(GameManager.instance.upgradePickups[Random.Range(0, GameManager.instance.upgradePickups.Count)], transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.up));
                GameManager.instance.upgradePickups.Remove(droppedItem);
            }
            else if(GameManager.instance.enemiesKilled % 10 == 0)
            {
                Instantiate(GameManager.instance.tempPickups[Random.Range(0, GameManager.instance.tempPickups.Count)], transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.up));
            }
        }
        health = maxHP;
        enemyKilled = 1;
        CreditsValue = creditsValueStart;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
