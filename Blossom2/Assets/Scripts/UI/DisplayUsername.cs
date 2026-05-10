using Firebase.Auth;
using TMPro;
using UnityEngine;

public class DisplayUsername : MonoBehaviour
{
    [SerializeField] TMP_Text usernameText;

    private System.EventHandler authStateHandler;
    private static string pendingStatusMessage;
    private static DisplayUsername instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(pendingStatusMessage))
        {
            usernameText.text = pendingStatusMessage;
            pendingStatusMessage = null;
        }
        else
        {
            DisplayCurrentUsername();
        }

        authStateHandler = (sender, args) => DisplayCurrentUsername();
        FirebaseAuth.DefaultInstance.StateChanged += authStateHandler;
    }

    void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= authStateHandler;
    }

    void DisplayCurrentUsername()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user == null)
        {
            usernameText.text = "Guest";
            return;
        }

        usernameText.text = string.IsNullOrEmpty(user.DisplayName)
            ? user.Email
            : user.DisplayName;
    }

    public static void SetLoadingMessage(string message)
    {
        pendingStatusMessage = message;
    }
}