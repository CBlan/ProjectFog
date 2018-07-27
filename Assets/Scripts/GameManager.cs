using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject player;
    public List<GameObject> enemies;

    public PatrolArea rangedPatArea;
    public PatrolArea meleePatArea;

    // Use this for initialization
    void Awake () {
        instance = this;
	}
	
}
