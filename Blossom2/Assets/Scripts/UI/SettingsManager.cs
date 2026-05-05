using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsManager: MonoBehaviour
{
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown localisationDropdown;

    public void Start()
    {
        OnChangeMasterVolume();
        OnChangeMusicVolume();
        OnChangeSFXVolume();
    }
    public void OnChangeMasterVolume()
    {
        audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
    }
    public void OnChangeMusicVolume()
    {
        audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
    }
    public void OnChangeSFXVolume()
    {
        audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
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