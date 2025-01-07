using Firebase.Auth;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Extensions;

public class GoogleAuthStrategy : IAuthStrategy
{
    private readonly FirebaseAuth auth;
    private readonly string webClientId; // Your Google Web Client ID

    public GoogleAuthStrategy(string clientId)
    {
        auth = FirebaseAuth.DefaultInstance;
        webClientId = clientId;
    }

    public Task<FirebaseUser> Register()
    {
        throw new System.NotImplementedException();
    }

    public Task<FirebaseUser> SignIn()
    {
        try
        {
            GoogleSignInConfiguration config = new GoogleSignInConfiguration
            {
                WebClientId = webClientId,
                RequestEmail = true,
                RequestIdToken = true
            };

            GoogleSignIn.Configuration = config;
            Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
            TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();

            signIn.ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("task.IsCanceled");
                    signInCompleted.SetCanceled();
                }
                else if (task.IsFaulted)
                {
                    Debug.Log(task.Exception.Message);

                    signInCompleted.SetException(task.Exception);
                }
                else
                {
                    Debug.Log("credential");

                    var credential = GoogleAuthProvider.GetCredential(signIn.Result.IdToken, null);
                    var authResult = auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                    {
                        if (authTask.IsCanceled)
                        {
                            Debug.Log("authTask.IsCanceled");

                            signInCompleted.SetCanceled();
                        }
                        else if (authTask.IsFaulted)
                        {
                            Debug.Log("authTask.IsFaulted");

                            signInCompleted.SetException(authTask.Exception);
                        }
                        else
                        {
                            Debug.Log("else");

                            signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                        }
                    });
                }
            });
            return signInCompleted.Task;

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Google authentication failed: {e.Message}");
            throw;
        }
    }

    public Task SignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        auth.SignOut();
        return Task.CompletedTask;
    }
}
