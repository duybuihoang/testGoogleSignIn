using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : BaseMonobehavior
{
    private static MenuManager instance;
    public static MenuManager Instance => instance;

    [SerializeField] private List<MenuItem> menuItems = new List<MenuItem>();
    public event Action<MenuItem> OnItemUnlocked;

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

        //Debug.Log(Application.persistentDataPath + "/gamesave.dat");

        if (PlayerPrefs.GetInt("FirstTime", 0) == 0)
        {
            Debug.Log("FirstTime");
        }
        else
        {
            LoadUnlockedItems();
        }
    }



    private void LoadUnlockedItems()
    {   
        foreach (MenuItem item in menuItems)
        {
            Debug.Log(PlayerPrefs.GetInt($"MenuItem_{item.id}", 0));
            item.isUnlocked = PlayerPrefs.GetInt($"MenuItem_{item.id}", 0) == 1;
            Debug.Log(item.isUnlocked);
        }
    }

    public List<MenuItem> GetAllItems() => menuItems;

    private void SaveUnlockedItems()
    {
        foreach (MenuItem item in menuItems)
        {
            Debug.Log(item.isUnlocked);
            PlayerPrefs.SetInt($"MenuItem_{item.id}", item.isUnlocked ? 1 : 0);
        }
        PlayerPrefs.SetInt("FirstTime", 1);

        PlayerPrefs.Save();
    }

    public List<MenuItem> GetUnlockedItems()
    {
        return menuItems.FindAll(x => x.isUnlocked);
    }

    public bool UnlockMenuItem(string itemId)
    {
        MenuItem item = menuItems.Find(x => x.id == itemId);
        if (item == null || item.isUnlocked) return false;

        if (MoneyManager.Instance.SpendMoney(item.unlockCost))
        {
            item.isUnlocked = true;
            SaveUnlockedItems();
            OnItemUnlocked?.Invoke(item);
            return true;
        }
        return false;
    }

}
