using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    public GameObject pausePannel;
    public GameObject optionsPannel;
    public MouseLook cameraLook1;
    public MouseLook cameraLook2;
    public GrenadeThrower grenadeThrower;
    public HUDController hudController;
    bool paused = false;

	void Start () {
        Time.timeScale = 1;
        pausePannel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
	}

    public void TogglePause()
    {
        if (paused == true)
        {
            Time.timeScale = 1.0f;
            cameraLook1.enabled = true;
            cameraLook2.enabled = true;
            grenadeThrower.enabled = true;
            hudController.enabled = true;
            pausePannel.SetActive(false);
            optionsPannel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            paused = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            cameraLook1.enabled = false;
            cameraLook2.enabled = false;
            grenadeThrower.enabled = false;
            hudController.enabled = false;
            pausePannel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            paused = true;
        }
    }
}
