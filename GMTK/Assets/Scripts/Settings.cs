using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Canvas normalCanvas, settingsCanvas, keybindCanvas;

    [Header("Sliders")]
    public Slider mouseSensitivity, musicVolume, sfxVolume;

    [Header("Text Boxes")]
    public TextMeshProUGUI mouseSensitivityText, musicVolumeText, sfxVolumeText;

    [Header("Mixers")]
    public AudioMixer sfxMixer, musicMixer;

    public void MusicVolume(float musicSliderValue)
    {
        musicMixer.SetFloat("musicVol", Mathf.Log10(musicSliderValue) * 20);
    }
    public void SFXVolume(float sfxSliderValue)
    {
        sfxMixer.SetFloat("sfxVol", Mathf.Log10(sfxSliderValue) * 20);
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("Mouse Sensitivity")) PlayerPrefs.SetFloat("Mouse Sensitivity", 75);
        if (!PlayerPrefs.HasKey("Music Volume")) PlayerPrefs.SetFloat("Music Volume", 1);
        if (!PlayerPrefs.HasKey("SFX Volume")) PlayerPrefs.SetFloat("SFX Volume", 1);

        mouseSensitivity.value = PlayerPrefs.GetFloat("Mouse Sensitivity");
        musicVolume.value = PlayerPrefs.GetFloat("Music Volume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFX Volume");

        keybindCanvas.enabled = false;
    }

    void Update()
    {
        mouseSensitivityText.SetText("" + Mathf.RoundToInt(mouseSensitivity.value));
        musicVolumeText.SetText("Music: " + Mathf.RoundToInt(musicVolume.value * 100));
        sfxVolumeText.SetText("SFX: " + Mathf.RoundToInt(sfxVolume.value * 100));
    }
    public void Back()
    {
        normalCanvas.enabled = true;
        settingsCanvas.enabled = false;

        PlayerPrefs.SetFloat("Mouse Sensitivity", mouseSensitivity.value);
        PlayerPrefs.SetFloat("Music Volume", musicVolume.value);
        PlayerPrefs.SetFloat("SFX Volume", sfxVolume.value);
    }
    public void Keybinds()
    {
        settingsCanvas.enabled = false;
        keybindCanvas.enabled = true;
        keybindCanvas.GetComponent<Keybinds>().OpenPage();
    }
}
