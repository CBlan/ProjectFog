using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject player;
    public GameObject[] tutorialPannels;
    private int activeTutorialPannel;
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
	
    public void NextPannel()
    {
        tutorialPannels[activeTutorialPannel].SetActive(false);
        activeTutorialPannel++;
        if (activeTutorialPannel > tutorialPannels.Length-1)
        {
            print("Tutorial Finished");
        }
        else
        {
            tutorialPannels[activeTutorialPannel].SetActive(true);
        }
    }

}
