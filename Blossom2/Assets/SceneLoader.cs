using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;
    private bool isGlobal = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        if (gameObject.name == "GlobalEmpty")
        {
            isGlobal = true;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnLoadAuth()
    {
        /*
        #if UNITY_EDITOR
        FirebaseAuth.DefaultInstance.SignOut();
        #endif  
        */
        SceneManager.LoadScene("Authentication");
    }
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
