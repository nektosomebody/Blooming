using System;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class DatabaseConnection : MonoBehaviour
{
    public static DatabaseConnection Instance { get; private set; }

    DatabaseReference root;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            root = FirebaseDatabase.DefaultInstance.RootReference;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task SaveAvatarAsync(string uid, int index)
    {
        try
        {
            await root.Child("users").Child(uid).Child("avatarIndex").SetValueAsync(index);
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveAvatar failed: {e}");
            throw;
        }
    }

    public async Task<int> LoadAvatarAsync(string uid)
    {
        try
        {
            var snapshot = await root.Child("users").Child(uid).Child("avatarIndex").GetValueAsync();
            return snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"LoadAvatar failed: {e}");
            return 0;
        }
    }

    public async Task SaveUsernameAsync(string uid, string username)
    {
        try
        {
            await root.Child("users").Child(uid).Child("username").SetValueAsync(username);
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveUsername failed: {e}");
            throw;
        }
    }

    public async Task<string> LoadUsernameAsync(string uid)
    {
        try
        {
            var snapshot = await root.Child("users").Child(uid).Child("username").GetValueAsync();
            return snapshot.Exists ? snapshot.Value.ToString() : string.Empty;
        }
        catch (Exception e)
        {
            Debug.LogError($"LoadUsername failed: {e}");
            return string.Empty;
        }
    }

    public async Task SaveScoreAsync(string uid, string levelType, int score)
    {
        try
        {
            var path = root.Child("users").Child(uid).Child("scores").Child(levelType);
            var snapshot = await path.GetValueAsync();

            bool isNewRecord = !snapshot.Exists || score > Convert.ToInt32(snapshot.Value);
            if (isNewRecord)
                await path.SetValueAsync(score);
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveScore failed: {e}");
            throw;
        }
    }

    public async Task<int> LoadScoreAsync(string uid, string levelType)
    {
        try
        {
            var snapshot = await root.Child("users").Child(uid).Child("scores").Child(levelType).GetValueAsync();
            return snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"LoadScore failed: {e}");
            return 0;
        }
    }

    public async Task InitUserProfileAsync(string uid, string username)
    {
        try
        {
            await root.Child("users").Child(uid).SetValueAsync(new System.Collections.Generic.Dictionary<string, object>
            {
                { "username",    username },
                { "avatarIndex", 0        }
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"InitUserProfile failed: {e}");
            throw;
        }
    }
}
