using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour {

    void Start()
    {
        Fabric.EventManager.Instance.PostEvent("Ambient/GameMusic");
    }
}
