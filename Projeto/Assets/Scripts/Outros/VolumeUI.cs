using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    void Start()
    {
        // Carrega os valores salvos
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        uiSlider.value = PlayerPrefs.GetFloat("UIVolume", 1f);

        // Adiciona os listeners chamando o AudioManager
        musicSlider.onValueChanged.AddListener(v => AudioManager.instance.UpdateMusicVolume(v));
        sfxSlider.onValueChanged.AddListener(v => AudioManager.instance.UpdateSFXVolume(v));
        uiSlider.onValueChanged.AddListener(v => AudioManager.instance.UpdateUIVolume(v));
    }
}
