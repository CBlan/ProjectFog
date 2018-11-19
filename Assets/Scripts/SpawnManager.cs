using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public int meleeEnemies, rangedEnemies, scoutEnemies;
    public GameObject rangedPrefab, meleePrefab, scoutPrefab;
    public float timeBetweenSpawns;
    public float timeIncreaseRanged, timeIncreaseMelee, timeIncreaseScout;
    public Transform[] spawnPositions;

    //private int totalEnemies;

    public int currentMelee, currentRanged, currentScout;

    // Use this for initialization
    void Start () {
        //totalEnemies = meleeEnemies + scoutEnemies + rangedEnemies;
        StartCoroutine(IncreaseRangedEnemies());
        StartCoroutine(IncreaseMeleeEnemies());
        //StartCoroutine(IncreaseScoutEnemies());
        StartCoroutine(SpawnEnemies());
    }
	

    IEnumerator IncreaseRangedEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIncreaseRanged);
            rangedEnemies++;
            if (rangedEnemies == 30)
            {
                yield break;
            }
            //totalEnemies++;
            yield return null;
        }
    }

    IEnumerator IncreaseMeleeEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIncreaseMelee);
            meleeEnemies++;
            if (meleeEnemies == 30)
            {
                yield break;
            }
            //totalEnemies++;
            yield return null;
        }
    }

    IEnumerator IncreaseScoutEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIncreaseScout);
            scoutEnemies++;
            //totalEnemies++;
            yield return null;
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {

            if (currentRanged < rangedEnemies)
            {
                for (int i = currentRanged; i < rangedEnemies; i++)
                {
                    SpawnEnemy("Ranged");
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
            }
            if (currentMelee < meleeEnemies)
            {
                for (int i = currentMelee; i < meleeEnemies; i++)
                {
                    SpawnEnemy("Melee");
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
            }
            if (currentScout < scoutEnemies)
            {
                for (int i = currentScout; i < scoutEnemies; i++)
                {
                    SpawnEnemy("Scout");
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
            }
            yield return null;
        }
    }

    //void SpawnEnemy(GameObject enemy)
    //{
    //    Transform point = spawnPositions[Random.Range(0, spawnPositions.Length-1)];
    //    Instantiate(enemy, point.position, point.rotation);
    //}

    void SpawnEnemy(string type)
    {
        for (int i = 0; i < GameManager.instance.enemies.Count; i++)
        {
            if (GameManager.instance.enemies[i].CompareTag(type) && !GameManager.instance.enemies[i].activeSelf)
            {
                GameManager.instance.enemies[i].transform.position = spawnPositions[Random.Range(0, spawnPositions.Length - 1)].position;
                GameManager.instance.enemies[i].SetActive(true);
                break;
            }
        }
    }
}
