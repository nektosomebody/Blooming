using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        if (audioMixer == null) return;
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat(SettingsManager.MasterVolumeKey, -40f));
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat(SettingsManager.MusicVolumeKey, 0f));
        audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat(SettingsManager.SfxVolumeKey, 0f));

        RestoreLocalization();
    }

    private void RestoreLocalization()
    {
        if (!LocalizationSettings.InitializationOperation.IsDone)
        {
            Debug.Log("[Localization] Waiting for initialization...");
            LocalizationSettings.InitializationOperation.WaitForCompletion();
        }

        string savedLocale = PlayerPrefs.GetString(SettingsManager.LocaleKey, "English");
        Debug.Log($"[Localization] Restoring locale: {savedLocale}");

        string localeCode = SettingsManager.GetLocalIdentifier(savedLocale);
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(localeCode));

        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            Debug.Log($"[Localization] Applied: {locale.LocaleName}");
        }
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
