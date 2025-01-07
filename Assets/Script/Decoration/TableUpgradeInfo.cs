public class TableUpgradeInfo
{
    public string TableId { get; set; }
    public int CurrentLevel { get; set; }
    public float CurrentIncome { get; set; }
    public float UnlockCost { get; set; }    // Cost to unlock (level 0 to 1)
    public float UpgradeCost { get; set; }   // Cost to upgrade (level 1+)
    public bool CanUnlock { get; set; }      // Can unlock from level 0
    public bool CanUpgrade { get; set; }     // Can upgrade if already unlocked
}