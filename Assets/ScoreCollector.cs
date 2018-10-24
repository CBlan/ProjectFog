using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollector : MonoBehaviour {

    public static ScoreCollector score;
    public float time;
    public int enemiesKilled;
    public float credits;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        score = this;
    }
}
