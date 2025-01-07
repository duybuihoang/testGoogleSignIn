using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : SpawnManager
{
    private static NPCSpawner instance;
    public static NPCSpawner Instance { get => instance; }

    public static List<string> npcName = new List<string>();


    protected override void Awake()
    {
        base.Awake();
        if (NPCSpawner.instance != null) Debug.LogError("Only 1 NPCSpawner allow to exist");
        NPCSpawner.instance = this;
    }

    private void Start()
    {
        npcName.Add("NPC");
    }
}
