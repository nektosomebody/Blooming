using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    public GameObject[] AllLevels;
    public int Number;

    public void GoToNext(int num)
    {
        for (int i = 0; i < AllLevels.Length; i++)
        {
            AllLevels[i].SetActive(false);
        }

        Number = (Number + num + AllLevels.Length) % AllLevels.Length;
        AllLevels[Number].SetActive(true);       
    }
}
