using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundNaidLvl5 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Fabric.EventManager.Instance.PostEvent("Player/Grenade/Lvl5", Fabric.EventAction.PlaySound, gameObject);
    }
	
}
