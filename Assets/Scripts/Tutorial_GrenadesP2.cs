using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_GrenadesP2 : MonoBehaviour {

    private bool destroyed;

    public GameObject enemy;
    private GameObject ene;

    private void Start()
    {
        ene = Instantiate(enemy, Tutorial.controller.player.transform.position, Tutorial.controller.player.transform.rotation);
        ene.transform.parent = this.transform;
    }

    void Update()
    {
        if (!ene.transform.GetChild(0).gameObject.activeSelf && !destroyed)
        {
            Tutorial.controller.NextPannel();
            destroyed = true;
        }
    }
}
