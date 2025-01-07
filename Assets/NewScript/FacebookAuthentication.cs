using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Facebook.Unity;
using System.Collections.Generic;
using Firebase.Extensions;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using Firebase.Auth;

public class FacebookAuthentication : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    FirebaseAuth authManager;

    protected void Awake()
    {

        // FB.Init(SetInit, OnHideUnity);

        if (!FB.IsInitialized)
        {
            FB.Init(() => {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    print("Coudn't initialize");
            }, isGameShown => {
                if (!isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
            FB.ActivateApp();
    }

    protected void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            print("Facebook is Login!");
            string s = "client token" + FB.ClientToken + "User Id" + AccessToken.CurrentAccessToken.UserId;
        }
        else
        {
            print("Facebook is not Logged in!");
        }
        DealWithFbMenu(FB.IsLoggedIn);
    }

    void DealWithFbMenu(bool IsLoggedIn)
    {
        if (IsLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUserName);
            FB.API("/me/picture?type=square&redirect=false&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
        else
        {
            print("Not logged in");
        }
    }

    void DisplayUserName(IResult result)
    {
        if (result.Error == null)
        {
            string name = "" + result.ResultDictionary["first_name"];
           // authManager.nameTxt.text = name;
           // authManager.LoggedIn();
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {
        if (result == null)
        {
            Debug.LogError("Result is null.");
            return;
        }

        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.LogError("Error: " + result.Error);
            return;
        }

        if (result.Cancelled)
        {
            Debug.LogWarning("Request was cancelled.");
            return;
        }

        if (result.Texture != null)
        {
            var data = result.ResultDictionary["data"] as Dictionary<string, object>;
            string url = data["url"] as string;
            StartCoroutine(LoadProfilePic(url));
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    IEnumerator LoadProfilePic(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                //authManager.avatar.sprite = sprite;
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
    }

    public void LoginWithFaceBook()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    public void LogOut()
    {
        auth.SignOut();
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            SetInit();
            // AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;

            print(aToken.UserId);
            FacebookAuth(aToken.TokenString);

            foreach (string perm in aToken.Permissions)
            {
                print(perm);
            }

        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void FacebookAuth(string accessToken)
    {
        Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });

    }
}