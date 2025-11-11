using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer Principal")]
    public AudioMixer audioMixer;

    [Header("Controles de Volume")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    public TMP_Text musicText;
    public TMP_Text sfxText;
    public TMP_Text uiText;

    void Start()
    {
        // Inicializa sliders
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        uiSlider.onValueChanged.AddListener(SetUIVolume);

        // Define valores padrão
        musicSlider.value = 0.8f;
        sfxSlider.value = 0.8f;
        uiSlider.value = 0.8f;

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetUIVolume(uiSlider.value);
    }

    void SetMusicVolume(float value)
    {
        float volume = Mathf.Lerp(-80f, 0f, value);
        audioMixer.SetFloat("MusicVolume", volume);
        musicText.text = $"Música: {(int)(value * 100)}%";
    }

    void SetSFXVolume(float value)
    {
        float volume = Mathf.Lerp(-80f, 0f, value);
        audioMixer.SetFloat("SFXVolume", volume);
        sfxText.text = $"SFX: {(int)(value * 100)}%";
    }

    void SetUIVolume(float value)
    {
        float volume = Mathf.Lerp(-80f, 0f, value);
        audioMixer.SetFloat("UIVolume", volume);
        uiText.text = $"UI: {(int)(value * 100)}%";
    }
}