using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour {

	void Start () {
        Fabric.EventManager.Instance.PostEvent("Ambient/MenuMusic");
    }
	
}
