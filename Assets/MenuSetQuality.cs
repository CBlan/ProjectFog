using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSetQuality : MonoBehaviour {

    public Toggle[] qualityToggles;
    private int currentQuality;

    private void Start()
    {
        currentQuality = QualitySettings.GetQualityLevel();
        qualityToggles[currentQuality].isOn = true;
    }

    private void Update()
    {
        for (int i = 0; i < qualityToggles.Length; i++)
        {
            if (qualityToggles[i].isOn)
            {
                QualitySettings.SetQualityLevel(i, true);
            }
        }
    }

}
