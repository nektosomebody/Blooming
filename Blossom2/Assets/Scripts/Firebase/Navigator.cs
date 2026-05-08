using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] ErrorOperator errorOperator;

    private Authentication registration;

    private void Awake()
    {
        registration = new Authentication();
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
            await registration.ForgetPassword(emailToChangePass.text);
        }
        catch (Exception e)
        {
            errorOperator.ShowFirebaseError(e, ErrorContext.ForgetPassword);
        }
    }

    private async Task SignInUser()
    {
        errorOperator.ClearErrors(ErrorContext.SignIn);

        string emailErr = InputValidator.ValidateEmail(signInEmail.text);
        string passErr = InputValidator.ValidatePassword(signInPassword.text);
        errorOperator.ShowSignInValidation(emailErr, passErr);
        if (emailErr != null || passErr != null) return;

        try
        {
            await registration.SignInUser(signInEmail.text, signInPassword.text);
            Debug.Log("User signed in successfully.");
            LoadNextScene();
        }
        catch (Exception e)
        {
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
            await registration.CreateUser(registerEmail.text, registerPassword.text, registerName.text);
            Debug.Log("User created successfully.");
            LoadNextScene();
        }
        catch (Exception e)
        {
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
