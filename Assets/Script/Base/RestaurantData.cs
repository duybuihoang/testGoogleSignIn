using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RestaurantData
{
    public bool firstTime;
    public int tableAmounts = 1;

    public List<TableData> tablesInfo = new List<TableData>();
}
[System.Serializable]
public class TableData
{
    public int level = 0;
}
