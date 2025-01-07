using System;
using UnityEngine;

public class MoneyManager : BaseMonobehavior    
{
    private static MoneyManager instance;
    public static MoneyManager Instance => instance;

    [SerializeField] private float currentMoney = 1000f;

    public delegate void MoneyChangedHandler(float currentMoney);
    public event MoneyChangedHandler OnMoneyChanged;

    private void OnEnable()
    {   

    }

    protected override void Awake()
    {
        Debug.Log("MoneyManager");  
        if (Instance == null) { instance = this; }
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadMoney();
    }

    public bool CanAfford(float amount) => currentMoney >= amount;

    public void AddMoney(float amount)
    {
        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney);
        SaveMoney();
    }

    public bool SpendMoney(float amount)
    {
        if (!CanAfford(amount)) return false;

        currentMoney -= amount;
        OnMoneyChanged?.Invoke(currentMoney);
        SaveMoney();
        return true;
    }

    private void SaveMoney()
    {
        //PlayerPrefs.SetFloat("PlayerGold", currentMoney);
        GameData data = new GameData(currentMoney, true);
        SaveSystem.SaveGame(data);
        PlayerPrefs.Save();
    }

    private void LoadMoney()
    {
        //currentMoney = PlayerPrefs.GetFloat("PlayerGold", 1000f );
        GameData loadedData = SaveSystem.LoadGame();
        if(loadedData != null)
        {
            currentMoney = loadedData.money;
            OnMoneyChanged?.Invoke(currentMoney);
        }
        else
        {
            currentMoney = 1000f;
            OnMoneyChanged?.Invoke(currentMoney);
        }    


    }

}
