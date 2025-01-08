using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Facebook.Unity;
using Firebase.Extensions;
using System;
using Firebase;


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
                var credential = FacebookAuthProvider.GetCredential(token);
                var authResult = await auth.SignInWithCredentialAsync(credential);

                authTaskCompletionSource.SetResult(authResult);
            }
            catch (FirebaseException e ) when((AuthError)e.ErrorCode == AuthError.AccountExistsWithDifferentCredentials)
            {
                    FB.API("/me?fields=email", HttpMethod.GET,async fbResult =>
                    {
                        if (!string.IsNullOrEmpty(fbResult.Error))
                        {
                            Debug.LogError($"Error fetching email: {fbResult.Error}");
                            authTaskCompletionSource.SetException(new Exception(fbResult.Error));
                        }
                        else
                        {
                            try
                            {
                                var email = fbResult.ResultDictionary["email"].ToString();
                                Debug.Log($"Fetched email: {email}");
                                var providers = await auth.FetchProvidersForEmailAsync(email);
                                Debug.Log(providers);
                                var availableProviders = string.Join(", ", providers);
                                var errorMessage = $"This email is already registered. Please sign in using one of these methods: {availableProviders}";
                                Debug.LogWarning(errorMessage);
                                authTaskCompletionSource.SetException(new Exception(errorMessage));
                            }
                            catch (Exception innerEx)
                            {
                                Debug.LogError($"Error while handling existing account: {innerEx.Message}");
                                authTaskCompletionSource.SetException(innerEx);
                            }
                        }
                    });

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
