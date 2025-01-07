using UnityEngine;

public class StandardIncomeStrategy : IIncomeStrategy
{
    public float CalculateIncome(float baseIncome, int level)
    {
        return baseIncome * (1 + (level * 0.5f));
    }
}
