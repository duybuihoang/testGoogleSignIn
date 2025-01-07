using System;
using System.Collections;
using UnityEngine;

public class Table : BaseGridObject
{
    private bool isOccupied;
    private NPC currentCustomer;
    public bool IsOccupied  { get => isOccupied; set => isOccupied = value; }

    [SerializeField] private float baseIncome = 10f;
    [SerializeField] private float baseUpgradeCost = 2000;
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private Sprite[]    levelSprites;

    private SpriteRenderer spriteRenderer;

    private TableData data = new TableData();
    public TableData Data { get => data; set => data = value; }
    //private int currentLevel = 1;
    private IIncomeStrategy incomeStrategy;


    public float NextUpgradeCost => baseUpgradeCost * Mathf.Pow(2f, data.level);
    public bool CanUpgrade => data.level < maxLevel;
    public bool IsUnlocked => data.level > 0;
    public int CurrentLevel => data.level;
    public NPC CurrentCusstomer => currentCustomer;
    public float CurrentIncome =>  incomeStrategy.CalculateIncome(baseIncome, data.level) ;



    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        incomeStrategy = new StandardIncomeStrategy();
        //LoadLevel();
        UpdateVisuals();
    }   

    public void SetLevel(int level)
    {
        Data.level = level;
        UpdateVisuals();

    }

    public bool TryUpgrade()
    {
        if (!CanUpgrade) return false;

        float cost = NextUpgradeCost;
        if (MoneyManager.Instance.SpendMoney(cost))
        {
            data.level++;
            //SaveLevel();
            UpdateVisuals();

            return true;
        }
        return false;
    }

    public override bool CanMove(int targetX, int targetY) => false;

    public bool TryOccupyTable(NPC customer)
    {
        if (IsOccupied) return false;
        IsOccupied = true;
        currentCustomer = customer;
        return true;
    }

    public void ReleaseTable()
    {
        IsOccupied = false;
        currentCustomer = default;
    }

    private void UpdateVisuals()
    {
        // Update sprite based on level
        if (spriteRenderer != null && levelSprites != null && levelSprites.Length > 0)
        {
            int spriteIndex = Mathf.Clamp(data.level - 1, 0, levelSprites.Length - 1);
            if (spriteIndex >= 0 && spriteIndex < levelSprites.Length)
            {
                spriteRenderer.sprite = levelSprites[spriteIndex];
            }
        }
    }

}
