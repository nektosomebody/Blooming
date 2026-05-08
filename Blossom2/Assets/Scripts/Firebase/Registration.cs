using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Registration
{
    FirebaseAuth auth;
    FirebaseUser user;

    public Registration()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    internal async Task CreateUser(string email, string password, string userName)
    {
        try
        {
            var task = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            Debug.Log($"User signed in successfully: {task.User.UserId}");
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", task.User.DisplayName, task.User.UserId);
            await UpdateUsersProfile(userName);
            await DatabaseConnection.Instance.InitUserProfileAsync(task.User.UserId, userName);
        }
        catch (Exception e)
        {
            Debug.LogError("Login failed: " + e);
            throw;
        }
    }

    /*
    test user:
    email = "u@mail.ru";
    password = "Test123";
     */
    internal async Task SignInUser(string email, string password)
    {

        try
        {
            var task = await auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.Log($"User signed in successfully: {task.User.UserId}");
        }
        catch (Exception e)
        {
            Debug.LogError("Login failed: " + e);
            throw;
        }
    }
    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser
                && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    private async Task UpdateUsersProfile(string userName)
    {
        FirebaseUser user = auth.CurrentUser;

        if (user == null)
            return;

        var profile = new UserProfile
        {
            DisplayName = userName
        };

        try
        {
            await user.UpdateUserProfileAsync(profile);
            Debug.Log("User profile updated successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to update profile: " + e);
            throw;
        }
    }


    public async Task ForgetPassword(string email)
    {
        try
        {
            await auth.SendPasswordResetEmailAsync(email);
            Debug.Log("Password reset email sent successfully.");
        }
        catch (Firebase.FirebaseException e)
        {
            Debug.LogError("Firebase error: " + e);
            throw;
        }
        catch (Exception e)
        {
            Debug.LogError("Unknown error: " + e);
            throw;
        }
    }

    internal void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

}