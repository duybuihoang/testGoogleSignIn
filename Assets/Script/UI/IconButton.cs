using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;

public class IconButton : MonoBehaviour
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
        FacebookButton.onClick.AddListener(FacebookAuthen);
        XButton.onClick.AddListener(XAuthen);   
    }

    public void GoogleAuthen()
    {
        

        GoogleSignIn.Configuration = new GoogleSignInConfiguration  
        {
            RequestIdToken = true,
            // Copy this value from the google-service.json file.
            // oauth_client with type == 3
            WebClientId = "729184886702-2l71knis0pvvner2q0dftop7a4tgsr4b.apps.googleusercontent.com", 
            RequestEmail = true
        };
        Debug.Log("configure");
        
        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
        Debug.Log("SignIn");


        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWith(task => {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);
            }
            else
            {

                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask => {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                    }
                });
            }
        });


    }

    public void FacebookAuthen()
    {

    }   
    
    public void XAuthen()
    {

    }    

}
