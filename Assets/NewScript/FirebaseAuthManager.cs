using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour
{
    private static FirebaseAuthManager instance;
    private AuthStrategyFactory strategyFactory;
    private IAuthStrategy currentStrategy;
    private readonly List<IAuthObserver> observers = new List<IAuthObserver>();

    public static FirebaseAuthManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("FirebaseAuthManager");
                instance = go.AddComponent<FirebaseAuthManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public void Initialize(string googleClientID)
    {
        strategyFactory = new AuthStrategyFactory(googleClientID);
        FirebaseAuth.DefaultInstance.StateChanged += AuthStateChanged;

    }

    public void AddObserver(IAuthObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }
    public void RemoveObserver(IAuthObserver observer)
    {
        observers.Remove(observer);
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        foreach (var observer in observers)
        {
            observer.OnAuthStateChanged(user);
        }
    }

    public async Task<FirebaseUser> SignInWith(AuthProvider provider)
    {
        try
        {
            currentStrategy = strategyFactory.CreateStrategy(provider);
            var user = await currentStrategy.SignIn();
            return user;
        }
        catch (Exception e)
        {
            foreach (var observer in observers)
            {
                observer.OnAuthError(e.Message);
            }
            throw;
        }
    }

    public async Task<FirebaseUser> Register(AuthProvider provider)
    {
        try
        {
            currentStrategy = strategyFactory.CreateStrategy(provider);
            var user = await currentStrategy.Register();
            return user;


        }
        catch (Exception e)
        {
            foreach (var observer in observers)
            {
                observer.OnAuthError(e.Message);
            }
            throw;
        }

    }

    public void SetEmailPassword(string email, string password)
    {
        strategyFactory.SetEmailPassword(email, password);
    }    

    public async Task SignOut()
    {
        if (currentStrategy != null)
        {
            await currentStrategy.SignOut();
            currentStrategy = null;
        }
    }
 
}
