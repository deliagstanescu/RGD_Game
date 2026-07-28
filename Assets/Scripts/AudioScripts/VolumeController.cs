using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer mainMixer;

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    private const string MASTER_KEY = "MasterVolume";
    private const string SFX_KEY = "SFXVolume";
    private const string MUSIC_KEY = "MusicVolume";

    void Start()
    {
        if (masterSlider != null)
        {
            SetupSlider(masterSlider, MASTER_KEY, "MasterVolumePref");
            masterSlider.onValueChanged.AddListener(val => SetMasterVolume(val));
        }
        if (sfxSlider != null)
        {
            SetupSlider(sfxSlider, SFX_KEY, "SFXVolumePref");
            sfxSlider.onValueChanged.AddListener(val => SetSFXVolume(val));
        }
        if (musicSlider != null)
        {
            SetupSlider(musicSlider, MUSIC_KEY, "MusicVolumePref");
            musicSlider.onValueChanged.AddListener(val => SetMusicVolume(val));
        }
    }
    private void SetupSlider(Slider slider, string exposedParam, string prefKey)
    {
        slider.minValue = 0.0001f;
        slider.maxValue = 1f;
        float savedVolume = PlayerPrefs.GetFloat(prefKey, 0.75f);
        slider.value = savedVolume;
        ApplyVolume(exposedParam, savedVolume);
    }
    public void SetMasterVolume(float value)
    {
        ApplyVolume(MASTER_KEY, value);
        PlayerPrefs.SetFloat("MasterVolumePref", value);
    }
    public void SetSFXVolume(float value)
    {
        ApplyVolume(SFX_KEY, value);
        PlayerPrefs.SetFloat("SFXVolumePref", value);
    }
    public void SetMusicVolume(float value)
    {
        ApplyVolume(MUSIC_KEY, value);
        PlayerPrefs.SetFloat("MusicVolumePref", value);
    }
    private void ApplyVolume(string exposedParam, float sliderValue)
    {
        float volumeInDecibels = Mathf.Log10(sliderValue) * 20;
        mainMixer.SetFloat(exposedParam, volumeInDecibels);
    }
}