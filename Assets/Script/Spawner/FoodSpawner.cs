using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : SpawnManager
{
    private static FoodSpawner instance;
    public static FoodSpawner Instance { get => instance; }

    public static List<string> foodName = new List<string>();

    protected override void Awake() 
    {
        base.Awake();
        if (FoodSpawner.instance != null) Debug.LogError("Only 1 FoodSpawner allow to exist");
        FoodSpawner.instance = this;
    }
}
