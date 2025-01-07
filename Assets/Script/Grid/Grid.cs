using UnityEngine;

public class Grid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public Vector3 GetWorldPosition(Vector2Int xy)
    {
        return new Vector3(xy.x , xy.y) * cellSize + originPosition;
    }

    public Vector2Int GetXY(Vector3 worldPosition)
    {


        return new Vector2Int(
            Mathf.RoundToInt((worldPosition - originPosition).x / cellSize),
            Mathf.RoundToInt((worldPosition - originPosition).y / cellSize)
            );
    }
    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }
    public TGridObject GetValue(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return default;
    }

    public TGridObject GetValue(Vector2Int cell)
    {
        if (cell.x >= 0 && cell.y >= 0 && cell.x < width && cell.y < height)
        {
            return gridArray[cell.x, cell.y];
        }
        return default;
    }


}
