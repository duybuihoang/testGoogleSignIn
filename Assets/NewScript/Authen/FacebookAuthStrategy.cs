using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Facebook.Unity;
using Firebase.Extensions;
using System;

public class FacebookAuthStrategy : IAuthStrategy
{
    private readonly FirebaseAuth auth;
    private TaskCompletionSource<FirebaseUser> authTaskCompletionSource;

    public FacebookAuthStrategy()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public Task<FirebaseUser> SignIn()
    {
        authTaskCompletionSource = new TaskCompletionSource<FirebaseUser>();

        try
        {
            if (!FB.IsInitialized)
            {
                Debug.Log("try to init");
                FB.Init(OnFacebookInitialize);
            }
            else
            {
                OnFacebookInitialize();
            }
            return authTaskCompletionSource.Task;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Facebook authentication failed: {e.Message}");
            authTaskCompletionSource.SetException(e);
            return authTaskCompletionSource.Task;
        }
    }

    private void OnFacebookInitialize()
    {
        Debug.Log("FB Init");
        var permission = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permission, OnFacebookLoginComplete);
    }
    
    private async void OnFacebookLoginComplete(ILoginResult result)
    {
        if(result == null || string.IsNullOrEmpty(result.Error))
        {
            try
            {
                Debug.Log("signinng");

                var token = AccessToken.CurrentAccessToken.TokenString;
                Debug.Log(token);
                var credential = FacebookAuthProvider.GetCredential(token);
                Debug.Log(credential);

                var authResult = await auth.SignInWithCredentialAsync(credential);
                authTaskCompletionSource.SetResult(authResult);
                Debug.Log("signin");
            }
            catch (Exception e)
            {
                Debug.LogError($"Firebase auth with Facebook credential failed: {e.Message}");
                authTaskCompletionSource.SetException(e);
            }
        }    
        else
        {
            Debug.LogError($"Facebook login failed: {result.Error}");
            authTaskCompletionSource.SetException(new Exception(result.Error));
        }
    }

    public Task SignOut()
    {
        if(FB.IsLoggedIn)
        {
            FB.LogOut();
        }

        auth.SignOut();
        return Task.CompletedTask;
    }

    public Task<FirebaseUser> Register()
    {
        throw new NotImplementedException();
    }
}
