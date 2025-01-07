using System.Collections.Generic;
using UnityEngine;

public class TableSpawner : SpawnManager
{
    private static TableSpawner instance;
    public static TableSpawner Instance { get => instance; }

    public static List<string> TableName = new List<string>();

    protected override void Awake()
    {
        base.Awake();
        if (TableSpawner.instance != null) Debug.LogError("Only 1 TableSpawner allow to exist");
        TableSpawner.instance = this;
    }
    private void Start()
    {
        TableName.Add("TABLE");
    }
}
