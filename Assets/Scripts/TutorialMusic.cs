using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMusic : MonoBehaviour {

    void Start()
    {
        Fabric.EventManager.Instance.PostEvent("Ambient/Music");
    }
}
