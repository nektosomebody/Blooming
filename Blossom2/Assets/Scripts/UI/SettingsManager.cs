using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsManager: MonoBehaviour
{
    public const string MasterVolumeKey = "MasterVolume";
    public const string MusicVolumeKey  = "MusicVolume";
    public const string SfxVolumeKey    = "SFXVolume";

    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown localisationDropdown;

    public void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey, masterVolumeSlider.value);
        musicVolumeSlider.value  = PlayerPrefs.GetFloat(MusicVolumeKey,  musicVolumeSlider.value);
        sfxVolumeSlider.value    = PlayerPrefs.GetFloat(SfxVolumeKey,    sfxVolumeSlider.value);

        OnChangeMasterVolume();
        OnChangeMusicVolume();
        OnChangeSFXVolume();
    }
    public void OnChangeMasterVolume()
    {
        audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat(MasterVolumeKey, masterVolumeSlider.value);
    }
    public void OnChangeMusicVolume()
    {
        audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolumeSlider.value);
    }
    public void OnChangeSFXVolume()
    {
        audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolumeSlider.value);
    }
    public void OnChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    public void OnChangeLocal()
    {
        int ind = localisationDropdown.value;
        string local = localisationDropdown.options[ind].text;
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(GetLocalIdentifier(local)));
        LocalizationSettings.SelectedLocale = locale;
    }

    private string GetLocalIdentifier(string local)
    {
        switch (local)
        {
            case "English":
                return "en";
            case "Русский":
                return "ru";
            default:
                return "en";
        }
    }

}