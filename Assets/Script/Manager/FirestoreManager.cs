using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using System.Threading.Tasks;

public class FirestoreManager : MonoBehaviour
{
    private static FirestoreManager instance;
    public static FirestoreManager Instance { get => instance; }
    private FirebaseFirestore firestoreDb;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeFirestore();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeFirestore()
    {
        Debug.Log("Initializing Firestore");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    // Initialize Firestore
                    firestoreDb = FirebaseFirestore.DefaultInstance;

                    FirebaseFirestore.DefaultInstance.Settings.PersistenceEnabled = true;
                    Debug.Log("Firestore Initialized.");
                }
                else
                {
                    Debug.LogError("Could not resolve Firebase dependencies: " + task.Result);
                }
            }
        );
    }

    public FirebaseFirestore GetFirestore()
    {
        return firestoreDb;
    }
}