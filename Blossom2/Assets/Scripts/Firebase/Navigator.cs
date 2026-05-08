using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    public GameObject RegisterWnd;
    public GameObject ForgetPassWnd;
    public GameObject SignInWnd;

    // Registration fields
    public TMPro.TMP_InputField registerName;
    public TMPro.TMP_InputField registerEmail;
    public TMPro.TMP_InputField registerPassword;
    public TMPro.TMP_InputField confirmPassword;

    // Registration error labels (assign in Inspector)
    [SerializeField] private TMPro.TMP_Text registerNameError;
    [SerializeField] private TMPro.TMP_Text registerEmailError;
    [SerializeField] private TMPro.TMP_Text registerPasswordError;
    [SerializeField] private TMPro.TMP_Text confirmPasswordError;
    [SerializeField] private TMPro.TMP_Text registerGeneralError;

    // Sign-in fields
    public TMPro.TMP_InputField signInEmail;
    public TMPro.TMP_InputField signInPassword;

    // Sign-in error labels (assign in Inspector)
    [SerializeField] private TMPro.TMP_Text signInEmailError;
    [SerializeField] private TMPro.TMP_Text signInPasswordError;
    [SerializeField] private TMPro.TMP_Text signInGeneralError;

    // Forgot password field
    public TMPro.TMP_InputField emailToChangePass;

    // Forgot password error label (assign in Inspector)
    [SerializeField] private TMPro.TMP_Text forgetPassError;

    private Registration registration;

    private void Awake()
    {
        registration = new Registration();
    }

    // Navigation
    public void GoToChangePassword()
    {
        SignInWnd.SetActive(false);
        ForgetPassWnd.SetActive(true);
    }
    public void GoToRegistration()
    {
        SignInWnd.SetActive(false);
        RegisterWnd.SetActive(true);
    }
    public void GoToSignIn()
    {
        RegisterWnd.SetActive(false);
        ForgetPassWnd.SetActive(false);
        SignInWnd.SetActive(true);
    }

    // Button handlers
    public void OnSignIn()              => _ = SignInUser();
    public void OnSignUp()              => _ = CreateUser();
    public void OnForgetPasswordClicked() => _ = ForgetPassword();

    private async Task ForgetPassword()
    {
        ClearErrors(forgetPassError);

        string emailErr = InputValidator.ValidateEmail(emailToChangePass.text);
        if (emailErr != null) { ShowError(forgetPassError, emailErr); return; }

        try
        {
            await registration.ForgetPassword(emailToChangePass.text);
        }
        catch (Exception e)
        {
            ShowError(forgetPassError, GetFirebaseMessage(e));
        }
    }

    private async Task SignInUser()
    {
        ClearErrors(signInEmailError, signInPasswordError, signInGeneralError);

        string emailErr = InputValidator.ValidateEmail(signInEmail.text);
        string passErr  = InputValidator.ValidatePassword(signInPassword.text);

        if (emailErr != null) ShowError(signInEmailError, emailErr);
        if (passErr  != null) ShowError(signInPasswordError, passErr);
        if (emailErr != null || passErr != null) return;

        try
        {
            await registration.SignInUser(signInEmail.text, signInPassword.text);
            LoadNextScene();
        }
        catch (Exception e)
        {
            ShowError(signInGeneralError, GetFirebaseMessage(e));
        }
    }

    private async Task CreateUser()
    {
        ClearErrors(registerNameError, registerEmailError,
                    registerPasswordError, confirmPasswordError, registerGeneralError);

        string nameErr  = InputValidator.ValidateName(registerName.text);
        string emailErr = InputValidator.ValidateEmail(registerEmail.text);
        string passErr  = InputValidator.ValidatePassword(registerPassword.text);
        bool   match    = registerPassword.text == confirmPassword.text;

        if (nameErr  != null) ShowError(registerNameError,     nameErr);
        if (emailErr != null) ShowError(registerEmailError,    emailErr);
        if (passErr  != null) ShowError(registerPasswordError, passErr);
        if (!match)           ShowError(confirmPasswordError,  "Пароли не совпадают");

        if (nameErr != null || emailErr != null || passErr != null || !match) return;

        try
        {
            await registration.CreateUser(registerEmail.text, registerPassword.text, registerName.text);
            LoadNextScene();
        }
        catch (Exception e)
        {
            ShowError(registerGeneralError, GetFirebaseMessage(e));
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void OnDestroy()
    {
        registration.OnDestroy();
    }

    // Helpers
    private static void ShowError(TMPro.TMP_Text label, string message)
    {
        if (label != null) label.text = message;
    }

    private static void ClearErrors(params TMPro.TMP_Text[] labels)
    {
        foreach (var label in labels)
            if (label != null) label.text = string.Empty;
    }

    private static string GetFirebaseMessage(Exception e)
    {
        if (e is Firebase.FirebaseException firebaseEx)
        {
            switch ((Firebase.Auth.AuthError)firebaseEx.ErrorCode)
            {
                case Firebase.Auth.AuthError.UserNotFound:      return "Пользователь не найден";
                case Firebase.Auth.AuthError.WrongPassword:
                case Firebase.Auth.AuthError.InvalidCredential: return "Неверный пароль";
                case Firebase.Auth.AuthError.EmailAlreadyInUse: return "Email уже используется";
                case Firebase.Auth.AuthError.InvalidEmail:      return "Неверный формат email";
                default:                                        return "Ошибка авторизации";
            }
        }
        return "Произошла ошибка. Попробуйте снова";
    }
}
