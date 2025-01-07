using System.Collections.Generic;
using UnityEngine;

public class FloatingTextSpawner : SpawnManager
{
    private static FloatingTextSpawner instance;

    public static FloatingTextSpawner Instance { get => instance; }

    public static List<string> TextName = new List<string>();
    protected override void Awake()
    {
        base.Awake();
        if (FloatingTextSpawner.instance != null) Debug.LogError("Only 1 FloatingTextSpawner allow to exist");
        FloatingTextSpawner.instance = this;

    }

}
