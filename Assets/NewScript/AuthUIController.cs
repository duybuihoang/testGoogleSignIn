using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthUIController : MonoBehaviour, IAuthObserver
{
    [SerializeField] private Button FacebookButton;
    [SerializeField] private Button GoogleButton;
    [SerializeField] private Button AnonymousButton;
    [SerializeField] private Button Login;
    [SerializeField] private Button Register;

    [SerializeField] private TMP_InputField Username;
    [SerializeField] private TMP_InputField Password;



    private void Start()
    {


        FirebaseAuthManager.Instance.Initialize("729184886702-el87scsfm74k3be1mffj9i3g77e37375.apps.googleusercontent.com");
        FirebaseAuthManager.Instance.AddObserver(this);
    }

    private void OnEnable()
    {

        FacebookButton.onClick.AddListener(OnFacebookSignInClick);
        GoogleButton.onClick.AddListener(OnGoogleSignInClick);
        AnonymousButton.onClick.AddListener(OnAnonymousSignInClick);
        Login.onClick.AddListener(OnEmailSignInClick);
        Register.onClick.AddListener(OnEmailRegisterClick);
    }

    private void OnDestroy()
    {
        FirebaseAuthManager.Instance.RemoveObserver(this);
    }

    public void OnAuthStateChanged(FirebaseUser user)
    {
        if (user != null)
        {
            Debug.Log($"User signed in: {user.Email}");
            Debug.Log(user.UserId);
            Debug.Log(user.Email);
            Debug.Log(user.IsAnonymous);
            Debug.Log(user.Metadata);
            Debug.Log(user.PhotoUrl);
            Debug.Log(user.ProviderData);

            
            // Update UI for signed-in state
        }
        else
        {
            Debug.Log("User signed out");
            // Update UI for signed-out state
        }
    }

    public void OnAuthError(string error)
    {
        Debug.LogError($"Auth error: {error}");
        //Show UI
    }

    public async void OnGoogleSignInClick()
    {
        Debug.Log("click google");
        try
        {
            await FirebaseAuthManager.Instance.SignInWith(AuthProvider.Google);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async void OnFacebookSignInClick()
    {
        Debug.Log("click facebook");

        try
        {
            var user = await FirebaseAuthManager.Instance.SignInWith(AuthProvider.Facebook);
            SceneManager.LoadScene("Gameplay");

        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async void OnAnonymousSignInClick()
    {
        Debug.Log("click Anonymous");

        try
        {
            FirebaseUser User =  await FirebaseAuthManager.Instance.SignInWith(AuthProvider.Anonymous);
            Debug.Log("login: " + User.DisplayName);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async void OnEmailSignInClick()
    {
        Debug.Log("click email login");
        try
        {
            FirebaseAuthManager.Instance.SetEmailPassword(Username.text, Password.text);
            await FirebaseAuthManager.Instance.SignInWith(AuthProvider.Email);
        }
        catch (System.Exception e)
        {

            Debug.LogError(e);
        }
    }

    public async void OnEmailRegisterClick()
    {
        try
        {
            FirebaseAuthManager.Instance.SetEmailPassword(Username.text, Password.text);
            await FirebaseAuthManager.Instance.Register(AuthProvider.Email);
        }
        catch (System.Exception)
        {

        }
    }    
}
