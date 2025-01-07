using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : BaseMonobehavior
{
    [System.Serializable]
    public class SpawnableObject
    {
        public string id;
        public GameObject prefab;
    }

    [SerializeField] private List<SpawnableObject> spawnableObjects;
    private Dictionary<string, GameObject> spawnablePrefabs;


    protected override void Awake()
    {
        InitializeSpawnableDictionary();
    }

    private void InitializeSpawnableDictionary()
    {
        spawnablePrefabs = new Dictionary<string, GameObject>();
        foreach (var obj in spawnableObjects)
        {
            spawnablePrefabs[obj.id] = obj.prefab;
        }
    }

    public virtual GameObject SpawnObject(string objID, int x, int y, Transform parent = null)
    {
        if (!spawnablePrefabs.ContainsKey(objID))
        {
            Debug.LogError($"No spawnable object found with ID: {objID}");
            return default;
        }

        Grid<IGridObject> grid = GridManager.Instance.GetGrid();

        Vector3 worldPos = grid.GetWorldPosition(x, y);
        GameObject spawned = Instantiate(spawnablePrefabs[objID], worldPos, Quaternion.identity, parent);
        var gridObj = spawned.GetComponent<BaseGridObject>();

        if (gridObj != null)
        {
            gridObj.Initialize(grid, x, y);
            if(!spawned.GetComponent<NPC>())
            {
                gridObj.SetValue(x, y);
            }
            return spawned;
        }

        Debug.LogError($"Spawned object does not have a BaseGridObject component: {objID}");
        Destroy(spawned);
        return null;
    }

    public bool CanSpawnAt(int x, int y)
    {
        var grid = GridManager.Instance.GetGrid();

        return GridUtils.IsWithinBounds(x, y, GridManager.Instance.GetGridWidth(), GridManager.Instance.GetGridHeight())
            && grid.GetValue(x, y) == null;
    }    
}
