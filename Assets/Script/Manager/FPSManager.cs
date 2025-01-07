using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Database;

public class FPSManager : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private FirebaseApp app;
    private bool isFireBaseReady;
    private DatabaseReference databaseReference; // Tham chi?u t?i Realtime Database



    private void Awake()
    {
       
    }

    private void Start()
    {
       

    }

    private void GetDataFromDatabase()
    {
        databaseReference.Child("1").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // X? lý l?i
                Debug.LogError("Error while fetching data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                // L?y d? li?u thành công
                DataSnapshot snapshot = task.Result;
                Debug.Log("Snapshot: " + snapshot);

                foreach (DataSnapshot user in snapshot.Children)
                {
                    string userId = user.Key; // ID c?a user (user1, user2, ...)
                    string name = user.Child("name").Value.ToString(); // L?y tên
                    int age = int.Parse(user.Child("age").Value.ToString()); // L?y tu?i

                    Debug.Log($"User ID: {userId}, Name: {name}, Age: {age}");
                }
            }
        });
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;


        if(isFireBaseReady)
        {

        }    

    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        int width = Screen.width;
        int height = Screen.height;

        Rect rect = new Rect(10, 10, width, height * 2 / 100); 
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 100; 
        style.normal.textColor = Color.white;

        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS", fps);

        GUI.Label(rect, text, style);
    }
}
