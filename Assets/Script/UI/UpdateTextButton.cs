using UnityEngine;
using TMPro;

public class UpdateTextButton : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!RestaurantManager.Instance.IsMaxTable())
        {
            text.text = $"Next Unlock: {(int)RestaurantManager.Instance.GetTableUnlockCost()}";
        }
        else
        {
            text.text = $"Next Upgrade: {(int)RestaurantManager.Instance.GetUpgradeCost()}";
        }
    } 
}
