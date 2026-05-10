using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Navigator : MonoBehaviour
{

    [Header("Register inputs")]
    public TMPro.TMP_InputField registerName;
    public TMPro.TMP_InputField registerEmail;
    public TMPro.TMP_InputField registerPassword;
    public TMPro.TMP_InputField confirmPassword;

    [Header("Sign-in inputs")]
    public TMPro.TMP_InputField signInEmail;
    public TMPro.TMP_InputField signInPassword;

    [Header("Forgot password input")]
    public TMPro.TMP_InputField emailToChangePass;

    [Header("Status messages")]
    [SerializeField] TMP_Text registerStatusText;
    [SerializeField] TMP_Text signInStatusText;
    [SerializeField] TMP_Text forgetPasswordStatusText;

    [SerializeField] ErrorOperator errorOperator;

    private Authentication registration;

    private void Awake()
    {
        registration = new Authentication();
    }

    private void ShowStatus(TMP_Text statusText, string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.gameObject.SetActive(true);
            Debug.Log(message);
        }
    }

    private void HideStatus(TMP_Text statusText)
    {
        if (statusText != null)
        {
            statusText.gameObject.SetActive(false);
        }
    }

    public void OnSignIn() => _ = SignInUser();
    public void OnSignUp() => _ = CreateUser();
    public void OnForgetPasswordClicked() => _ = ForgetPassword();

    private async Task ForgetPassword()
    {
        errorOperator.ClearErrors(ErrorContext.ForgetPassword);

        string emailErr = InputValidator.ValidateEmail(emailToChangePass.text);
        errorOperator.ShowForgetPassValidation(emailErr);
        if (emailErr != null) return;

        try
        {
            ShowStatus(forgetPasswordStatusText, "Sending data...");
            await registration.ForgetPassword(emailToChangePass.text);
            ShowStatus(forgetPasswordStatusText, "Password reset email sent!");
            Invoke(nameof(HideForgetPasswordStatus), 2f);
        }
        catch (Exception e)
        {
            HideStatus(forgetPasswordStatusText);
            errorOperator.ShowFirebaseError(e, ErrorContext.ForgetPassword);
        }
    }

    private void HideForgetPasswordStatus() => HideStatus(forgetPasswordStatusText);

    private async Task SignInUser()
    {
        errorOperator.ClearErrors(ErrorContext.SignIn);

        string emailErr = InputValidator.ValidateEmail(signInEmail.text);
        string passErr = InputValidator.ValidatePassword(signInPassword.text);
        errorOperator.ShowSignInValidation(emailErr, passErr);
        if (emailErr != null || passErr != null) return;

        try
        {
            ShowStatus(signInStatusText, "Sending data...");
            await registration.SignInUser(signInEmail.text, signInPassword.text);
            DisplayUsername.SetLoadingMessage("Loading profile...");
            await Task.Delay(500);
            LoadNextScene();
        }
        catch (Exception e)
        {
            HideStatus(signInStatusText);
            errorOperator.ShowFirebaseError(e, ErrorContext.SignIn);
        }
    }

    private async Task CreateUser()
    {
        errorOperator.ClearErrors(ErrorContext.Register);

        string nameErr = InputValidator.ValidateName(registerName.text);
        string emailErr = InputValidator.ValidateEmail(registerEmail.text);
        string passErr = InputValidator.ValidatePassword(registerPassword.text);
        bool match = registerPassword.text == confirmPassword.text;

        errorOperator.ShowRegisterValidation(nameErr, emailErr, passErr, match);
        if (nameErr != null || emailErr != null || passErr != null || !match) return;

        try
        {
            ShowStatus(registerStatusText, "Sending data...");
            await registration.CreateUser(registerEmail.text, registerPassword.text, registerName.text);
            DisplayUsername.SetLoadingMessage("Loading profile...");
            await Task.Delay(500);
            LoadNextScene();
        }
        catch (Exception e)
        {
            HideStatus(registerStatusText);
            errorOperator.ShowFirebaseError(e, ErrorContext.Register);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        registration.OnDestroy();
    }
}
