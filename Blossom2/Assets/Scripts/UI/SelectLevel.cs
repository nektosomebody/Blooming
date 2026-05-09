using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] public GameObject[] AllLevels;
    [SerializeField] public string[] LevelScenes;
    public int Number;

    public void GoToNext(int num)
    {
        AllLevels[Number].SetActive(false);
        Number = (Number + num + AllLevels.Length) % AllLevels.Length;
        AllLevels[Number].SetActive(true);       
    }

    public void OnLevelSelected()
    {
        SceneManager.LoadScene(LevelScenes[Number]);
    }
}
