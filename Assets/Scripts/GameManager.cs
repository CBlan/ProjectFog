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
    public float regenRate = 1;

    public bool playerDamaged;
    // Use this for initialization
    void Awake () {
        instance = this;
	}

    private void Start()
    {
        playerHP = maxPlayerHP;
        StartCoroutine(RegenHP());
    }

    void Update()
    {
        playerDamaged = false;
    }

    public void DamagePlayer(float amount)
    {
        playerDamaged = true;
        playerHP -= amount;
    }

    IEnumerator RegenHP()
    {
        while (true)
        {
            if (playerDamaged)
            {
                yield return new WaitForSeconds(5);
            }
            else
            {
                playerHP += regenRate;
                Mathf.Clamp(playerHP, 0, maxPlayerHP);
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }
    }
}
