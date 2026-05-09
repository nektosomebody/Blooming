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

            int currentTotal = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;
            int newTotal = currentTotal + score;
            await path.SetValueAsync(newTotal);

            Debug.Log($"[Score] {levelType}: +{score} (Total: {newTotal})");
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

    public async Task SaveBestTimeAsync(string uid, string levelType, float seconds)
    {
        try
        {
            var path = root.Child("users").Child(uid).Child("bestTimes").Child(levelType);
            var snapshot = await path.GetValueAsync();
            bool isNewRecord = !snapshot.Exists || seconds < Convert.ToSingle(snapshot.Value);
            if (isNewRecord)
                await path.SetValueAsync((double)seconds);
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveBestTime failed: {e}");
            throw;
        }
    }

    public async Task<float> LoadBestTimeAsync(string uid, string levelType)
    {
        try
        {
            var snapshot = await root.Child("users").Child(uid).Child("bestTimes").Child(levelType).GetValueAsync();
            return snapshot.Exists ? Convert.ToSingle(snapshot.Value) : 0f;
        }
        catch (Exception e)
        {
            Debug.LogError($"LoadBestTime failed: {e}");
            return 0f;
        }
    }

    public void SaveBestTime(string uid, string levelType, float seconds, Action onDone = null)
    {
        _ = SaveBestTimeAsync(uid, levelType, seconds).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void LoadBestTime(string uid, string levelType, Action<float> onDone)
    {
        _ = LoadBestTimeAsync(uid, levelType).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke(t.Result);
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public async Task InitUserProfileAsync(string uid, string username)
    {
        try
        {
            await root.Child("users").Child(uid).SetValueAsync(new System.Collections.Generic.Dictionary<string, object>
            {
                { "username",    username },
                // { "avatarIndex", 0        }
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"InitUserProfile failed: {e}");
            throw;
        }
    }

    public void SaveAvatar(string uid, int index, Action onDone = null)
    {
        _ = SaveAvatarAsync(uid, index).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void LoadAvatar(string uid, Action<int> onDone)
    {
        _ = LoadAvatarAsync(uid).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke(t.Result);
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void SaveUsername(string uid, string username, Action onDone = null)
    {
        _ = SaveUsernameAsync(uid, username).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void LoadUsername(string uid, Action<string> onDone)
    {
        _ = LoadUsernameAsync(uid).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke(t.Result);
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void SaveScore(string uid, string levelType, int score, Action onDone = null)
    {
        _ = SaveScoreAsync(uid, levelType, score).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void LoadScore(string uid, string levelType, Action<int> onDone)
    {
        _ = LoadScoreAsync(uid, levelType).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke(t.Result);
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void InitUserProfile(string uid, string username, Action onDone = null)
    {
        _ = InitUserProfileAsync(uid, username).ContinueWith(t =>
        {
            if (t.IsFaulted) Debug.LogError(t.Exception);
            else onDone?.Invoke();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
}
