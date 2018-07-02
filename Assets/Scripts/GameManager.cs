using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject player;
    public List<GameObject> enemies;

    // Use this for initialization
    void Awake () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
