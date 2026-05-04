using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    public GameObject RegisterWnd;
    public GameObject ForgetPassWnd;
    public GameObject SignInWnd;

    public TMPro.TMP_InputField registerName;
    public TMPro.TMP_InputField registerEmail;
    public TMPro.TMP_InputField registerPassword;
    public TMPro.TMP_InputField confirmPassword;

    public TMPro.TMP_InputField signInEmail;
    public TMPro.TMP_InputField signInPassword;

    public TMPro.TMP_InputField emailToChangePass;

    private Registration registration;

    private void Awake()
    {
        registration = new Registration();
    }

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

    public void OnSignIn()
    {
        _ = SignInUser();
    }
    public void OnSignUp()
    {
        _ = CreateUser();
    }
    public void OnForgetPasswordClicked()
    {
        _ = ForgetPassword();
    }
    private async Task ForgetPassword()
    {
        await registration.ForgetPassword(emailToChangePass.text);
    }
    private async Task SignInUser()
    {
        await registration.SignInUser(signInEmail.text, signInPassword.text);
        LoadNextScene();
    }
    private async Task CreateUser()
    {
        Debug.Log(registerPassword.text);
        Debug.Log(confirmPassword.text);
        if (registerPassword.text == confirmPassword.text)
        {
            await registration.CreateUser(registerEmail.text, registerPassword.text, registerName.text);
            LoadNextScene();
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
}
