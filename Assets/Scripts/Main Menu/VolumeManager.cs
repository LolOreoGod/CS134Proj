using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    private string musicParam = "MusicVol";
    private string sfxParam = "SFXVol";

    // Start is called before the first frame update
    void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float val) {
        float dB = Mathf.Log10(val) * 20;
        mixer.SetFloat(musicParam, dB);
    }

    public void SetSFXVolume(float val) {
        float dB = Mathf.Log10(val) * 20;
        mixer.SetFloat(sfxParam, dB);
    }
}
