using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour {

    public AudioMixer masterMixer;
    public Slider master;
    public Slider effects;
    public Slider music;

    private float value;

    private void Start()
    {
        masterMixer.GetFloat("Master", out value);
        master.value = value;
        masterMixer.GetFloat("Effects", out value);
        effects.value = value;
        masterMixer.GetFloat("Music", out value);
        music.value = value;

    }

    public void SetMasterVol(float masterVol)
    {
        masterMixer.SetFloat("Master", masterVol);
    }

    public void SetSFXVol(float effectsVol)
    {
        masterMixer.SetFloat("Effects", effectsVol);
    }

    public void SetMusicVol(float musicVol)
    {
        masterMixer.SetFloat("Music", musicVol);
    }
}
