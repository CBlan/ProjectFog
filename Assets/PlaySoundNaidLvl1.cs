using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundNaidLvl1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Fabric.EventManager.Instance.PostEvent("Player/Grenade/Lvl1", Fabric.EventAction.PlaySound, gameObject);
    }
	
}
