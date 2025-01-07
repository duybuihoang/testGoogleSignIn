using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;

public class Analytics : Singleton<Analytics>
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            { 
                Debug.Log("Firebase Analytics is ready!");  
            }
            else
            {
                Debug.LogError($"Firebase initialization failed: {task.Result}");
            }
        });
    }

    public void LogCustomEvent(string param, string value)
    {
        FirebaseAnalytics.LogEvent("foodMenu", new Parameter[]
        {
        new Parameter(param, value),
        });
    }
}
