using UnityEngine;
using Firebase.Auth;

public class Authens : MonoBehaviour
{
    FirebaseAuth auth;

    FirebaseUser user;


    void Start()
    {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void RegisterUser(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("bad");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync is destroyed.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("account firebase created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    public void LoginUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync is destroyed.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync g?p l?i " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("signin: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });

        user = auth.CurrentUser;
        Debug.Log(user.Email);
        Debug.Log(user.DisplayName);
        Debug.Log(user.UserId);


    }

    public void LogoutUser()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("logout");
        }
    }


}
