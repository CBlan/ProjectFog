using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

    private float currentTime;
    private float currentEnemies;
    private float currentCredits;
    private float currentTotal;
    private int total;

    public Text timeScore;
    public Text enemiesScore;
    public Text creditsScore;
    public Text totalScore;

    void Start () {
        StartCoroutine(UpdateScore());
	}

    IEnumerator UpdateScore()
    {
        bool done = false;
        while (!done)
        {
            if (currentTime < ScoreCollector.score.time)
            {
                currentTime += ScoreCollector.score.time / 50;
            }
            timeScore.text = Mathf.RoundToInt(currentTime).ToString();

            if (currentEnemies < ScoreCollector.score.enemiesKilled)
            {
                currentEnemies += ScoreCollector.score.enemiesKilled / 50;
            }
            enemiesScore.text = Mathf.RoundToInt(currentEnemies).ToString();

            if (currentCredits < ScoreCollector.score.credits)
            {
                currentCredits += ScoreCollector.score.credits / 50;
            }
            creditsScore.text = Mathf.RoundToInt(currentCredits).ToString();
            yield return new WaitForSecondsRealtime(0.1f);

            if (currentTime >= ScoreCollector.score.time && currentEnemies >= ScoreCollector.score.enemiesKilled && currentCredits >= ScoreCollector.score.credits)
            {
                done = true;
            }
        }
        total = (Mathf.RoundToInt(currentTime)) + (Mathf.RoundToInt(currentEnemies)*100);
        StartCoroutine(TotalScoreUpdate());
        yield break;

    }

    IEnumerator TotalScoreUpdate()
    {
        while (currentTotal < total)
        {
            currentTotal += total / 50;
            totalScore.text = Mathf.RoundToInt(currentTotal).ToString();
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield break;
    }
}
