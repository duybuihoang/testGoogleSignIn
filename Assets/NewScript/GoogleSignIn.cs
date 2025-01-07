using Firebase.Auth;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
public class GoogleSignInButton : MonoBehaviour
{
    [SerializeField] private Button GoogleButton;
    [SerializeField] private Button FacebookButton;
    [SerializeField] private Button XButton;

    FirebaseAuth auth;
    FirebaseUser user;
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        GoogleButton.onClick.AddListener(GoogleAuthen);


        

    }
    public void GoogleAuthen()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            // Copy this value from the google-service.json file.
            // oauth_client with type == 3
            WebClientId = "729184886702-el87scsfm74k3be1mffj9i3g77e37375.apps.googleusercontent.com",
            //WebClientId = "729184886702-2l71knis0pvvner2q0dftop7a4tgsr4b.apps.googleusercontent.com",
            RequestEmail = true,
            UseGameSignIn = false
        };

        Debug.Log("configure");

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
        Debug.Log("SignIn");


        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWithOnMainThread(task => {
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

                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask => {
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
    }    
    public void FacebookAuthen()
    {
        
    }    
}
