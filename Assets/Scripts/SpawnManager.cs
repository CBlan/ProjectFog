using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public int meleeEnemies, rangedEnemies, scoutEnemies;
    public GameObject rangedPrefab, meleePrefab, scoutPrefab;
    public float timeBetweenSpawns;
    public float timeIncreaseRanged, timeIncreaseMelee, timeIncreaseScout;
    public Transform[] spawnPositions;

    private int totalEnemies;

    private int currentMelee, currentRanged, currentScout;

    // Use this for initialization
    void Start () {
        totalEnemies = meleeEnemies + scoutEnemies + rangedEnemies;
        StartCoroutine(IncreaseRangedEnemies());
        StartCoroutine(IncreaseMeleeEnemies());
        StartCoroutine(IncreaseScoutEnemies());
        StartCoroutine(SpawnEnemies());
    }
	

    IEnumerator IncreaseRangedEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIncreaseRanged);
            rangedEnemies++;
            totalEnemies++;
            yield return null;
        }
    }

    IEnumerator IncreaseMeleeEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIncreaseMelee);
            meleeEnemies++;
            totalEnemies++;
            yield return null;
        }
    }

    IEnumerator IncreaseScoutEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIncreaseScout);
            scoutEnemies++;
            totalEnemies++;
            yield return null;
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (GameManager.instance.enemies.Count < totalEnemies)
            {
                //print("here");
                foreach (GameObject enemy in GameManager.instance.enemies)
                {
                    if (enemy.GetComponent<Unit_Ranged>())
                    {
                        currentRanged++;
                    }

                    else if (enemy.GetComponent<Unit_Melee>())
                    {
                        currentMelee++;
                    }
                    
                    else 
                    {
                        currentScout++;
                    }
                }

                if (currentRanged < rangedEnemies)
                {
                    for (int i = currentRanged; i < rangedEnemies; i++)
                    {
                        SpawnEnemy(rangedPrefab);
                        yield return new WaitForSeconds(timeBetweenSpawns);
                    }
                }
                if (currentMelee < meleeEnemies)
                {
                    for (int i = currentMelee; i < meleeEnemies; i++)
                    {
                        SpawnEnemy(meleePrefab);
                        yield return new WaitForSeconds(timeBetweenSpawns);
                    }
                }
                if (currentScout < scoutEnemies)
                {
                    for (int i = currentScout; i < scoutEnemies; i++)
                    {
                        SpawnEnemy(scoutPrefab);
                        yield return new WaitForSeconds(timeBetweenSpawns);
                    }
                }

                currentScout = 0;
                currentMelee = 0;
                currentRanged = 0;
            }
            yield return null;
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        Transform point = spawnPositions[Random.Range(0, spawnPositions.Length-1)];
        Instantiate(enemy, point.position, point.rotation);
    }
}
