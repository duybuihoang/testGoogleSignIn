using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager
{
    private readonly List<Table> tables = new List<Table>();

    public void AddTable(Table table)
    {
        if (!tables.Contains(table))
        {
            tables.Add(table);
        }
    }

    public void ClearTable()
    {
        tables.Clear();
    }

    public  void RemoveTable(Table table)
    {
        tables.Remove(table);
    }

    public Table GetAvailableTable()
    {
        return tables.FirstOrDefault(t => !t.IsOccupied);
    }

    public Table SelectLowestLevel()
    { 
        var min = tables[0];
        foreach (var item in tables)
        {
            if (min.CurrentLevel > item.CurrentLevel)
                min = item;
        }

        return min;
    }

    public IReadOnlyList<Table> GetAllTables() => tables.AsReadOnly();

    public List<TableData> GetTablesData()
    {
        List<TableData> tableDatas = new List<TableData>();
        foreach (var table in tables)
        {
            tableDatas.Add(new TableData
            {
                level = table.Data.level 
            });
            Debug.Log(table.Data.level);
        }
        return tableDatas;
    }
}
