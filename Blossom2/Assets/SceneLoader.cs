using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] Button signInButton;
    [SerializeField] Button signOutButton;
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
            PreventDestroyAllChildren(transform);
        }
        OnAuthStateChanged();
    }

    private void OnAuthStateChanged()
    {
        Debug.Log("Auth state changed!!!");
        var auth = FirebaseAuth.DefaultInstance;
        bool isLoggedIn = auth.CurrentUser != null && auth.CurrentUser.IsValid();
        Debug.Log($"Auth state changed. User is logged in: {isLoggedIn}");

        signInButton.gameObject.SetActive(!isLoggedIn);
        signOutButton.gameObject.SetActive(isLoggedIn);
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
    public void OnSignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        DisplayUsername.instance.UpdateUsername();
        OnAuthStateChanged();
    }

    private void PreventDestroyAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            DontDestroyOnLoad(child.gameObject);
            PreventDestroyAllChildren(child);
        }
    }
}
