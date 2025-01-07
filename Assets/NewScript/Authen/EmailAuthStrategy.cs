using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EmailAuthStrategy : IAuthStrategy
{
    private readonly FirebaseAuth auth;
    private TaskCompletionSource<FirebaseUser> authTaskCompletionSource;
    private string email;
    private string password;

    public EmailAuthStrategy(string email, string password)
    {
        this.auth = FirebaseAuth.DefaultInstance;
        this.email = email;
        this.password = password;

    }


    public Task<FirebaseUser> Register()
    {
        authTaskCompletionSource = new TaskCompletionSource<FirebaseUser>();
        try
        {
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                FirebaseUser user = result.User;

                user.SendEmailVerificationAsync().ContinueWith(emailTask =>
                {
                    if(emailTask.IsFaulted)
                    {
                        Debug.LogError("Error sending verification email: " + emailTask.Exception);
                        authTaskCompletionSource.SetException(emailTask.Exception);
                        return;
                    }
                    else
                    {
                        Debug.Log("Verification email sent successfully.");
                        authTaskCompletionSource.SetResult(user);
                    } 
                });


                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    user.DisplayName, user.UserId);
            });
        }
        catch (System.Exception ex)
        {
            authTaskCompletionSource.SetException(ex);

            throw;
        }
        return authTaskCompletionSource.Task;
    }

    public Task<FirebaseUser> SignIn()
    {
        authTaskCompletionSource = new TaskCompletionSource<FirebaseUser>();
        try
        {
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }
        catch (System.Exception)
        {

            throw;
        }
        return authTaskCompletionSource.Task;

    }

    public Task SignOut()
    {
        throw new System.NotImplementedException();
    }
}
