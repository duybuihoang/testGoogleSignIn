using Firebase.Database;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using System.Threading.Tasks;

public class FireBaseManager : MonoBehaviour
{
    private static FireBaseManager instance;
    public static FireBaseManager Instance { get => instance; }

    private DatabaseReference dbReference;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            loadDBRef();
            //DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }    
    }

    private void loadDBRef()
    {
        Debug.Log("loading db");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    Debug.Log(FirebaseApp.DefaultInstance.Options.DatabaseUrl);
                    dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                    Debug.Log("Firebase Initialized.");

                }
                else
                {
                    Debug.LogError("Could not resolve Firebase dependencies: " + task.Result);
                }
            }

            );
    }    


    public DatabaseReference GetDatabaseReference()
    {
        return dbReference;
    }
}
