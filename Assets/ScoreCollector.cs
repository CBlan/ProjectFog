using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        if (score != this)
        {
            Destroy(gameObject);
        }
    }

}
