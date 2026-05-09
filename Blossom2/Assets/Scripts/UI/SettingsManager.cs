using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsManager : MonoBehaviour
{
    public const string MasterVolumeKey = "MasterVolume";
    public const string MusicVolumeKey = "MusicVolume";
    public const string SfxVolumeKey = "SFXVolume";
    public const string LocaleKey = "SelectedLocale";

    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown localisationDropdown;



    public void Start()
    {
        // Initialize Localization System
        if (!LocalizationSettings.InitializationOperation.IsDone)
        {
            Debug.Log("[Localization] Waiting for LocalizationSettings initialization...");
            LocalizationSettings.InitializationOperation.WaitForCompletion();
        }

        // Restore audio settings
        masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey, masterVolumeSlider.value);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, musicVolumeSlider.value);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SfxVolumeKey, sfxVolumeSlider.value);

        OnChangeMasterVolume();
        OnChangeMusicVolume();
        OnChangeSFXVolume();

        // Restore locale setting
        RestoreLocaleSelection();
    }

    private void RestoreLocaleSelection()
    {
        string savedLocale = PlayerPrefs.GetString(LocaleKey, "English");
        Debug.Log($"[Localization] Restoring saved locale: {savedLocale}");

        // Find dropdown option that matches saved locale
        for (int i = 0; i < localisationDropdown.options.Count; i++)
        {
            if (localisationDropdown.options[i].text == savedLocale)
            {
                localisationDropdown.value = i;
                Debug.Log($"[Localization] Set dropdown to: {savedLocale} (index {i})");
                return;
            }
        }

        // If not found, default to English
        Debug.LogWarning($"[Localization] Saved locale '{savedLocale}' not found, defaulting to English");
        localisationDropdown.value = 0;
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
        Debug.Log($"[Localization] Selected: {local}");

        string localeCode = GetLocalIdentifier(local);
        Debug.Log($"[Localization] Locale code: {localeCode}");

        if (LocalizationSettings.AvailableLocales == null)
        {
            Debug.LogError("[Localization] AvailableLocales is null!");
            return;
        }

        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(localeCode));

        if (locale == null)
        {
            Debug.LogError($"[Localization] Failed to get locale for code: {localeCode}");
            return;
        }

        Debug.Log($"[Localization] Setting locale: {locale.LocaleName}");
        LocalizationSettings.SelectedLocale = locale;

        // Save selected locale
        PlayerPrefs.SetString(LocaleKey, local);
        PlayerPrefs.Save();
        Debug.Log($"[Localization] Saved locale preference: {local}");
    }

    public static string GetLocalIdentifier(string local)
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