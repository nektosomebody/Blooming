using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class UsersProfile : MonoBehaviour
{
    [SerializeField] GameObject currAvatar;
    [SerializeField] GameObject changeAvatarWnd;
    [SerializeField] GameObject userAvatar;
    [SerializeField] TMP_Text statusText;
    DatabaseConnection db;
    bool isLoggedIn;
    FirebaseAuth auth;
    AvatarChanger avatarChanger;

    public void Start()
    {
        db = DatabaseConnection.Instance;
        auth = FirebaseAuth.DefaultInstance;
        avatarChanger = changeAvatarWnd.GetComponent<AvatarChanger>();
        auth.StateChanged += (s, e) => isLoggedIn = auth.CurrentUser != null;

    }
    public void OnClicked()
    {
        if (isLoggedIn)
        {
            Debug.Log($"{auth.CurrentUser} {auth.CurrentUser.DisplayName} {auth.CurrentUser.UserId}");
            try
            {
                db.LoadAvatar(auth.CurrentUser.UserId, index =>
            {
                avatarChanger.Init(auth.CurrentUser.DisplayName, index);
                ShowChangeAvatarWnd();
            });
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading avatar: {ex.Message}");
            }
            
            avatarChanger.onAvatarChanged.RemoveListener(AvatarChanged);
            avatarChanger.onAvatarChanged.AddListener(AvatarChanged);
        }
        else
        {
            GoToRegistration();
        }
    }

    private void AvatarChanged(AvatarData data)
    {
        ShowStatus("Sending data...");

        db.SaveAvatar(auth.CurrentUser.UserId, data.avatarIndex);
        db.SaveUsername(auth.CurrentUser.UserId, data.userName);

        ShowStatus("Waiting for confirmation...");

        auth.CurrentUser.UpdateUserProfileAsync(new UserProfile { DisplayName = data.userName }).ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                ShowStatus("Data saved successfully!");
                currAvatar.GetComponent<SpriteRenderer>().sprite = data.avatarSprite;

                Invoke(nameof(HideStatus), 2f);
            }
            else
            {
                ShowStatus("Error saving data. Please try again.");
            }
        });
    }

    private void ShowStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.gameObject.SetActive(true);
            Debug.Log(message);
        }
    }

    private void HideStatus()
    {
        if (statusText != null)
        {
            statusText.gameObject.SetActive(false);
        }
    }

    private void GoToRegistration()
    {
        SceneManager.LoadScene("Authentication");
    }

    private void ShowChangeAvatarWnd()
    {
        try
        {
            userAvatar.SetActive(false);
            changeAvatarWnd.SetActive(true);
            Debug.Log("Show change avatar window");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error showing change avatar window: {ex.Message}");
        }
        
    }
    void OnDestroy()
    {
        auth.StateChanged -= (s, e) => isLoggedIn = auth.CurrentUser != null;
    }
}