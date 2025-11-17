using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer Principal")]
    public AudioMixer audioMixer;

    [Header("Sliders de Volume")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    [Header("Valores Padrão")]
    public float defaultVolume = 0f; // 0 dB

    private void Start()
    {
        // Inicia sliders com valores atuais ou padrão
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        uiSlider.value = PlayerPrefs.GetFloat("UIVolume", 1f);

        UpdateMusicVolume(musicSlider.value);
        UpdateSFXVolume(sfxSlider.value);
        UpdateUIVolume(uiSlider.value);

        // Listeners
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        uiSlider.onValueChanged.AddListener(UpdateUIVolume);
    }

    public void UpdateMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void UpdateSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void UpdateUIVolume(float value)
    {
        audioMixer.SetFloat("UIVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("UIVolume", value);
    }
}
