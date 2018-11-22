using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

    public GameObject player;
    public GameObject[] tutorialPannels;
    private int activeTutorialPannel;
    private CanvasGroup canvasGroup;
    public static Tutorial controller;

    private void Awake()
    {
        controller = this;
    }

    // Use this for initialization
    void Start () {


        foreach (GameObject panel in tutorialPannels)
        {
            panel.SetActive(false);
        }
        tutorialPannels[0].SetActive(true);
        activeTutorialPannel = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Loading");
        }
    }

    public void NextPannel()
    {
        StopCoroutine(ChangePannel());
        StartCoroutine(ChangePannel());
    }

    IEnumerator ChangePannel()
    {
        if (activeTutorialPannel > tutorialPannels.Length - 1)
        {
            SceneManager.LoadScene("Loading");
            yield break;
        }
        canvasGroup = tutorialPannels[activeTutorialPannel].GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.2f);

        }
        //yield return new WaitForSeconds(5f);
        tutorialPannels[activeTutorialPannel].SetActive(false);
        activeTutorialPannel++;
        if (activeTutorialPannel > tutorialPannels.Length - 1)
        {
            SceneManager.LoadScene("Main");
            yield break;
        }
        else
        {
            tutorialPannels[activeTutorialPannel].SetActive(true);
        }
        yield break;
    }

}
