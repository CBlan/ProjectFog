using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHP : MonoBehaviour {

    public VendingMachine vendingScript;
    Image upgradeImage;
	// Use this for initialization
	void Start () {
        upgradeImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (vendingScript.lookingAt == this.gameObject)
        {
            upgradeImage.color = Color.green;
            if (Input.GetKey(KeyCode.E))
            {
                upgradeImage.color = Color.red;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {

                GameManager.instance.maxPlayerHP += 10;
            }
        }
        else upgradeImage.color = Color.white;
    }
}
