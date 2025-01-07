using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class RestaurantSaveLoad
{
    public void SaveData(RestaurantData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);

        FireBaseManager.Instance.GetDatabaseReference()?.Child("restaurantData")
            .SetRawJsonValueAsync(jsonData)
            .ContinueWithOnMainThread(
            task =>
            {
                if(task.IsCompleted)
                {
                    Debug.Log("Data save to firebase!");
                }    
                else
                {
                    Debug.LogError($"Failed to save data: {task.Exception}");
                }    
            }
            );
    }    

    public void LoadData(System.Action<RestaurantData> onDataLoaded)
    {
         FireBaseManager.Instance.GetDatabaseReference()
            ?.Child("restaurantData")
            .GetValueAsync()
            .ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Exists)
                    {
                        string jsonData = snapshot.GetRawJsonValue();
                        RestaurantData data = JsonUtility.FromJson<RestaurantData>(jsonData);
                        onDataLoaded?.Invoke(data);
                        Debug.Log("Data loaded from Firebase!");
                    }
                    else
                    {
                        onDataLoaded?.Invoke(null);
                        Debug.LogWarning("No data found in Firebase.");
                    }    
                }
                else
                {
                    onDataLoaded?.Invoke(null);
                    Debug.LogError($"Failed to load data: {task.Exception}");
                }    
            }
            );

    }    
}
