using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : BaseMonobehavior
{
    [SerializeField] private GameObject tablePrefab;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] public Vector2Int[] tablePositions = new Vector2Int[6];
    [SerializeField] private Transform SpawnPosition;

    private Vector2Int exitPosition;
    public Vector2Int ExitPosition { get => exitPosition; }


    private static RestaurantManager instance;
    public static RestaurantManager Instance => instance;

    public TableManager tableManager = new TableManager();
    private int unlockedTableCount = 1; // Start with 1 table

    [SerializeField] public float baseTableUnlockCost = 100f;

    private RestaurantData currentData;
    private RestaurantSaveLoad saveLoad;



    protected override void Awake()
    {

        base.Awake();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        exitPosition = new Vector2Int(GridManager.Instance.GetGridWidth() - 7,
        GridManager.Instance.GetGridHeight() - 1);

        //LoadUnlockedTableCount();

    }

    private void Start()
    {
        //InitializeTables();
        saveLoad = new RestaurantSaveLoad();
        currentData = new RestaurantData();
        StartCoroutine(LoadData());

    }



    private void Update()
    {
        CheckAndAssignTables();
    }

    private IEnumerator LoadData()
    {        
        bool isFirst = true;
        bool isCompleted = false;

        yield return new WaitUntil(() => FireBaseManager.Instance != null );
        Debug.Log("waiting");

        saveLoad.LoadData(
            data =>
            {
                Debug.Log("data: " + data);
                if(data != null)
                {
                    isFirst = false;
                }

                isCompleted = true;
            });
        yield return new WaitUntil(() => isCompleted);

        if (isFirst)
        {
            Debug.Log("isFirst");
            unlockedTableCount = 1;

            CreateTableAt(tablePositions[0].x, tablePositions[0].y);
        }
        else
        {
            Debug.Log("not first");

            isCompleted = false;

            saveLoad.LoadData(
            data =>
            {
                unlockedTableCount = data.tableAmounts;
                for (int i = 0; i < unlockedTableCount; i++)
                {

                    CreateTableAt(tablePositions[i].x, tablePositions[i].y, data.tablesInfo[i].level);
                }


            });




            yield return new WaitUntil(() => isCompleted);

            

            Debug.Log(unlockedTableCount);
        }

        Debug.Log(isFirst);


    }
    private void OnApplicationPause(bool pause)
    {
        
    }

    private void OnApplicationQuit()
    {
        SaveData();
        Debug.Log("Game data saved on exit.");
    }

    private void SaveData()
    {
        currentData.firstTime = false;
        currentData.tableAmounts = unlockedTableCount;
        currentData.tablesInfo = tableManager.GetTablesData();

        saveLoad.SaveData(currentData);

        
    }    

    private void LoadUnlockedTableCount()
    {
        unlockedTableCount = PlayerPrefs.GetInt("UnlockedTableCount", 1);
    }

    private void SaveUnlockedTableCount()
    {
        PlayerPrefs.SetInt("UnlockedTableCount", unlockedTableCount);
        PlayerPrefs.Save();
    }

    public void Upgrade()
    {
        if(unlockedTableCount < tablePositions.Length)
        {
            if (MoneyManager.Instance.SpendMoney(GetTableUnlockCost()))
            {
                unlockedTableCount++;
                CreateTableAt(tablePositions[unlockedTableCount - 1].x, (tablePositions[unlockedTableCount - 1].y));
                //SaveUnlockedTableCount();
            }
            else
            {
                Debug.Log("u broke!!!");
            }    

        }   
        else
        {
            tableManager.SelectLowestLevel().TryUpgrade();
        }    

    }
    public float GetUpgradeCost()
    {
        return tableManager.SelectLowestLevel().NextUpgradeCost;
    }

    public bool IsMaxTable()
    {
        return unlockedTableCount >= 6;
    }

    public float GetTableUnlockCost()
    {
        return baseTableUnlockCost * Mathf.Pow(2, unlockedTableCount);
    }



    private void CreateTableAt(int x, int y, int level = 1)
    {
        GameObject tableObj = TableSpawner.Instance.SpawnObject("TABLE", x, y).gameObject;
        Table table = tableObj.GetComponent<Table>();
        table.SetLevel(level);
        tableManager.AddTable(table);

    }    
    private void CheckAndAssignTables()
    {
        var availableTable = tableManager.GetAvailableTable();
        if (availableTable != null)
        {
            NPC nextCustomer = WaitingQueue.Instance.PopFromQueue();

            if (nextCustomer != null && WaitingQueue.Instance.IsNPCReadyInQueue(nextCustomer))
            {
                WaitingQueue.Instance.RemoveFromQueue();
                if (availableTable.TryOccupyTable(nextCustomer))
                {   
                    nextCustomer.AssignTable(availableTable);
                }    
            }
        }
    }

}
