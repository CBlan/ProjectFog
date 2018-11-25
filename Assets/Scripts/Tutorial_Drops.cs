using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Drops : MonoBehaviour {

    private bool pickupPicked;

    public GameObject pickUp;
    private GameObject pick;

    private void Start()
    {
        Vector3 playerPos = Tutorial.controller.player.transform.position;
        Vector3 playerDirection = Tutorial.controller.player.transform.forward;
        float spawnDistance = 5;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        pick = Instantiate(pickUp, spawnPos, Tutorial.controller.player.transform.rotation);
    }

    void Update()
    {
        if (pick == null && !pickupPicked)
        {
            Tutorial.controller.NextPannel();
            pickupPicked = true;
        }
    }
}
