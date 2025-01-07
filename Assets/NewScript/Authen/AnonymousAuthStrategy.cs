using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnonymousAuthStrategy : IAuthStrategy
{
    private readonly FirebaseAuth auth;
    private TaskCompletionSource<FirebaseUser> authTaskCompletionSource;
    public AnonymousAuthStrategy()
    {
        this.auth = FirebaseAuth.DefaultInstance;
    }

    public Task<FirebaseUser> Register()
    {
        throw new System.NotImplementedException();
    }

    public Task<FirebaseUser> SignIn()
    {
        authTaskCompletionSource = new TaskCompletionSource<FirebaseUser>();
        try
        {
            auth.SignInAnonymouslyAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInAnonymouslyAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }
        catch (System.Exception e)
        {

            Debug.LogError($"Anonymous authentication failed: {e.Message}");
            authTaskCompletionSource.SetException(e);
            return authTaskCompletionSource.Task;
        }
        

        return authTaskCompletionSource.Task;
    }

    public Task SignOut()
    {
        throw new System.NotImplementedException();
    }
}
