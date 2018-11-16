using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject player;
    public PlayerMoveV3 playerScript;
    public GrenadeThrower grenadeThrowerScript;
    public List<GameObject> enemies;

    public PatrolArea_Ranged rangedPatArea;
    public PatrolArea_Melee meleePatArea;

    public float playerHP;
    public float maxPlayerHP = 100;
    public float regenRate = 0.5f;
    private float regenCooldown;

    public float maxOxygen = 100;
    public float oxygen;
    public float oxygenDegenRate = 0.1f;
    private float oxygenCooldown;

    public float credits = 100;
    public int enemiesKilled;

    public bool playerDamaged;

    public SpawnManager sM;
    public ScoutPoints scoutPoints;

    public GameObject[] startingEnemies;

    public List<GameObject> upgradePickups;

    private float playtime;
    // Use this for initialization
    void Awake () {
        instance = this;
	}

    private void Start()
    {
        oxygen = maxOxygen;
        playerHP = maxPlayerHP;
        playerScript = player.GetComponent<PlayerMoveV3>();
        grenadeThrowerScript = player.GetComponentInChildren<GrenadeThrower>();

        if (startingEnemies != null)
        {
            foreach (GameObject enemy in startingEnemies)
            {
                enemy.SetActive(true);
            }
        }
    }

    void Update()
    {
        playtime += Time.deltaTime;

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

        if (oxygenCooldown > 0.1)
        {
            oxygen -= oxygenDegenRate;
            oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);
            oxygenCooldown = 0;
        }
        oxygenCooldown += Time.deltaTime;

        //gameover
        if (oxygen <= 0 || playerHP <= 0)
        {
            ScoreCollector.score.credits = credits;
            ScoreCollector.score.time = playtime;
            ScoreCollector.score.enemiesKilled = enemiesKilled;
            SceneManager.LoadScene("GameOver");
        }
    }

    public void DamagePlayer(float amount)
    {
        playerDamaged = true;
        playerHP -= amount;
    }



}
