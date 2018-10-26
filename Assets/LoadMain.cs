using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        StartCoroutine(LoadScene());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
    }
}
