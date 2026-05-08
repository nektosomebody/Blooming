using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        if (audioMixer == null) return;
        Debug.Log(PlayerPrefs.GetFloat(SettingsManager.MasterVolumeKey, 1234f));
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat(SettingsManager.MasterVolumeKey, -40f));
        audioMixer.SetFloat("MusicVolume",  PlayerPrefs.GetFloat(SettingsManager.MusicVolumeKey, 0f));
        audioMixer.SetFloat("SFXVolume",    PlayerPrefs.GetFloat(SettingsManager.SfxVolumeKey, 0f));
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
