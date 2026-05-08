using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class UsersProfile : MonoBehaviour
{
    [SerializeField] GameObject currAvatar;
    [SerializeField] GameObject dialogWnd;
    [SerializeField] GameObject changeAvatarWnd;
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
            db.LoadAvatar(auth.CurrentUser.UserId, index =>
            {
                avatarChanger.Init(auth.CurrentUser.DisplayName, index);
                ShowChangeAvatarWnd();
            });
            avatarChanger.onAvatarChanged.AddListener(AvatarChanged);
        }
        else
        {
            GoToRegistration();
        }
    }

    private void AvatarChanged(AvatarData data)
    {
        db.SaveAvatar(auth.CurrentUser.UserId, data.avatarIndex);
        db.SaveUsername(auth.CurrentUser.UserId, data.userName);
        auth.CurrentUser.UpdateUserProfileAsync(new UserProfile { DisplayName = data.userName });
        currAvatar.GetComponent<SpriteRenderer>().sprite = data.avatarSprite;
    }

    private void GoToRegistration()
    {
        SceneManager.LoadScene("Authentication");
    }

    private void ShowChangeAvatarWnd()
    {
        GetComponent<Renderer>().enabled = false;
        changeAvatarWnd.SetActive(true);
    }
    void OnDestroy()
    {
        auth.StateChanged -= (s, e) => isLoggedIn = auth.CurrentUser != null;
    }
}