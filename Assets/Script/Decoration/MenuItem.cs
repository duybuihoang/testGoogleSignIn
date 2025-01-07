using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "newMenuItemData", menuName = "Data/Item Data/Food",order =0)]
public class MenuItem : ScriptableObject
{
    public string id;
    public string foodName;
    public float basePrice;
    public float unlockCost;
    public bool isUnlocked;
    public Sprite icon;
}
