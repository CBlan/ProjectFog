using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundNaidLvl3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Fabric.EventManager.Instance.PostEvent("Player/Grenade/Lvl3", Fabric.EventAction.PlaySound, gameObject);
    }
	
}
