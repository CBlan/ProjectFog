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
        if (score != this  && score != null)
        {
            Destroy(gameObject);
        }
        else
        {
            score = this;
        }
    }

}
