using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject player;
    public List<GameObject> enemies;

    public PatrolArea rangedPatArea;
    public PatrolArea meleePatArea;

    public float playerHP;
    public float maxPlayerHP = 100;
    public float regenRate = 0.5f;
    private float regenCooldown;

    public bool playerDamaged;
    // Use this for initialization
    void Awake () {
        instance = this;
	}

    private void Start()
    {
        playerHP = maxPlayerHP;
    }

    void Update()
    {
        if (regenCooldown > 0.1)
        {
            playerHP += regenRate;
            playerHP = Mathf.Clamp(playerHP, 0, maxPlayerHP);
            regenCooldown = 0;
        }
        regenCooldown += Time.deltaTime;

        if (playerDamaged)
        {
            regenCooldown = -2;
        }

        playerDamaged = false;
    }

    public void DamagePlayer(float amount)
    {
        playerDamaged = true;
        playerHP -= amount;
    }

}
