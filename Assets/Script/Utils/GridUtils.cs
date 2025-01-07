using System.Collections.Generic;
using UnityEngine;

public static class GridUtils 
{
    public static bool IsWithinBounds(int x, int y, int width, int height)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public static float GetDistance(int x1, int y1, int x2, int y2)
    {
        return Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
    }

    public static List<Vector2Int> GetNeighbors(int x, int y, bool includeDiagonals = true)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(x + 1, y),
            new Vector2Int(x - 1, y),
            new Vector2Int(x, y + 1),
            new Vector2Int(x, y - 1)
        };

        if (includeDiagonals)
        {
            neighbors.AddRange(new List<Vector2Int>
            {
                new Vector2Int(x + 1, y + 1),
                new Vector2Int(x - 1, y + 1),
                new Vector2Int(x + 1, y - 1),
                new Vector2Int(x - 1, y - 1)
            });
        }

        return neighbors;
    }

    public static List<Vector2Int> GetLeftRightNeighbors(int x, int y, bool includeDiagonals = true)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(x + 1, y),
            new Vector2Int(x - 1, y),
        };

        if (includeDiagonals)
        {
            neighbors.AddRange(new List<Vector2Int>
            {
                new Vector2Int(x + 1, y + 1),
                new Vector2Int(x - 1, y + 1),
                new Vector2Int(x + 1, y - 1),
                new Vector2Int(x - 1, y - 1)
            });
        }

        return neighbors;
    }
}
