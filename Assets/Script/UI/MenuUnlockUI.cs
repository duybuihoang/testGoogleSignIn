using UnityEngine;
using UnityEngine.UI;

public class MenuUnlockUI : BaseMonobehavior
{
    [SerializeField] private GameObject menuItemPrefab;
    [SerializeField] private Transform menuItemContainer;
    [SerializeField] private GameObject menuPanel;

    private void Start()
    {
        LoadMenuItems();
        MoneyManager.Instance.OnMoneyChanged += UpdateAllItems;
    }

    private void LoadMenuItems()
    {
        var items = MenuManager.Instance.GetAllItems();
        foreach (var item in items)
        {
            CreateMenuItem(item);
        }
    }

    private void CreateMenuItem(MenuItem item)
    {
        GameObject menuItemObj = Instantiate(menuItemPrefab, menuItemContainer);
        MenuItemUIElement menuItemUI = menuItemObj.GetComponent<MenuItemUIElement>();
        menuItemUI.Initialize(item);
    }

    private void UpdateAllItems(float currentMoney)
    {
        foreach (MenuItemUIElement menuItem in menuItemContainer.GetComponentsInChildren<MenuItemUIElement>())
        {
            menuItem.UpdateUI();
        }
    }
}
