using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemUIElement : BaseMonobehavior
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    //[SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI unlockCostText;
    [SerializeField] private Button unlockButton;
    //[SerializeField] private GameObject lockedOverlay;

    private MenuItem menuItem;

    public void Initialize(MenuItem item)
    {
        menuItem = item;
        icon.sprite = item.icon;
        nameText.text = item.foodName;
        //priceText.text = $"${item.basePrice:F0}";
        unlockCostText.text = $"{item.unlockCost:F0}";

        unlockButton.onClick.AddListener(TryUnlock);
        UpdateUI();
    }
    public void UpdateUI()
    {
        bool canAfford = MoneyManager.Instance.CanAfford(menuItem.unlockCost);
        unlockButton.interactable = !menuItem.isUnlocked && canAfford;
        //lockedOverlay.SetActive(!menuItem.isUnlocked);
    }

    private void TryUnlock()
    {
        if (MenuManager.Instance.UnlockMenuItem(menuItem.id))
        {
            AudioManager.Instance.PlaySFX("purchase-succesful-ingame-230550");
            UpdateUI();
            Analytics.Instance.LogCustomEvent("food", menuItem.id);

            //PlayUnlockEffect();
        }
    }
}
