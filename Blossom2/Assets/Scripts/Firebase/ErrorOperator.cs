using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public enum ErrorContext { Register, SignIn, ForgetPassword }

public class ErrorOperator : MonoBehaviour
{
    [Header("Register labels")]
    [SerializeField] TMP_Text registerNameError;
    [SerializeField] TMP_Text registerEmailError;
    [SerializeField] TMP_Text registerPasswordError;
    [SerializeField] TMP_Text confirmPasswordError;
    [SerializeField] TMP_Text registerGeneralError;

    [Header("Sign-in labels")]
    [SerializeField] TMP_Text signInEmailError;
    [SerializeField] TMP_Text signInPasswordError;
    [SerializeField] TMP_Text signInGeneralError;

    [Header("Forgot password label")]
    [SerializeField] TMP_Text forgetPassError;

    [Header("Localized messages")]
    [SerializeField] LocalizedString errorUserNotFound;
    [SerializeField] LocalizedString errorWrongPassword;
    [SerializeField] LocalizedString errorEmailInUse;
    [SerializeField] LocalizedString errorInvalidEmail;
    [SerializeField] LocalizedString errorAuthGeneric;
    [SerializeField] LocalizedString errorGeneric;
    [SerializeField] LocalizedString errorPasswordsMismatch;

    public void ClearErrors(ErrorContext context)
    {
        switch (context)
        {
            case ErrorContext.Register:
                Clear(registerNameError, registerEmailError, registerPasswordError,
                      confirmPasswordError, registerGeneralError);
                break;
            case ErrorContext.SignIn:
                Clear(signInEmailError, signInPasswordError, signInGeneralError);
                break;
            case ErrorContext.ForgetPassword:
                Clear(forgetPassError);
                break;
        }
    }

    public void ShowRegisterValidation(string nameErr, string emailErr, string passErr, bool passwordsMatch)
    {
        if (nameErr != null) Show(registerNameError, nameErr);
        if (emailErr != null) Show(registerEmailError, emailErr);
        if (passErr != null) Show(registerPasswordError, passErr);
        if (!passwordsMatch) Show(confirmPasswordError, errorPasswordsMismatch.GetLocalizedString());
    }

    public void ShowSignInValidation(string emailErr, string passErr)
    {
        if (emailErr != null) Show(signInEmailError, emailErr);
        if (passErr != null) Show(signInPasswordError, passErr);
    }

    public void ShowForgetPassValidation(string emailErr)
    {
        if (emailErr != null) Show(forgetPassError, emailErr);
    }

    public void ShowFirebaseError(Exception e, ErrorContext context)
    {
        TMP_Text label = context switch
        {
            ErrorContext.Register     => registerGeneralError,
            ErrorContext.SignIn       => signInGeneralError,
            ErrorContext.ForgetPassword => forgetPassError,
            _                        => null
        };
        Show(label, GetFirebaseMessage(e));
    }

    private string GetFirebaseMessage(Exception e)
    {
        if (e is Firebase.FirebaseException firebaseEx)
        {
            return (Firebase.Auth.AuthError)firebaseEx.ErrorCode switch
            {
                Firebase.Auth.AuthError.UserNotFound                          => errorUserNotFound.GetLocalizedString(),
                Firebase.Auth.AuthError.WrongPassword or
                Firebase.Auth.AuthError.InvalidCredential                     => errorWrongPassword.GetLocalizedString(),
                Firebase.Auth.AuthError.EmailAlreadyInUse                    => errorEmailInUse.GetLocalizedString(),
                Firebase.Auth.AuthError.InvalidEmail                         => errorInvalidEmail.GetLocalizedString(),
                _                                                             => errorAuthGeneric.GetLocalizedString()
            };
        }
        return errorGeneric.GetLocalizedString();
    }

    private static void Show(TMP_Text label, string message)
    {
        if (label != null) label.text = message;
    }

    private static void Clear(params TMP_Text[] labels)
    {
        foreach (var label in labels)
            if (label != null) label.text = string.Empty;
    }
}
