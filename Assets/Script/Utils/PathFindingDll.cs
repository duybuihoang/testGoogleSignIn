using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class PathFindingDll 
{
    [DllImport("AStar.dll")]
    private static extern IntPtr FindPath(int startX, int startY, int endX, int endY,
    out int pathLength, [MarshalAs(UnmanagedType.LPArray)] bool[] isWalkableArray, int gridWidth, int gridHeight);

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, bool[,] walkableGrid)
    {
        Debug.Log("start finding path");
        int width = GridManager.Instance.GetGridWidth();
        int height = GridManager.Instance.GetGridHeight();

        int pathLength = 0;
        IntPtr pathPtr = default;

        bool[] flattenedGrid = new bool[width * height];
        Debug.Log(width);
        Debug.Log(height);
        Debug.Log(walkableGrid.GetLength(0));
        Debug.Log(walkableGrid.GetLength(1));


        /*        for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Debug.Log(i * height + width);
                    }
                }*/

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Debug.Log(i * height + j + " " + walkableGrid[i, j]);
                flattenedGrid[i * height + j] = walkableGrid[i, j];
            }
        }
        try
        {
            pathPtr = FindPath(start.x, start.y, end.x, end.y, out pathLength, flattenedGrid, width, height);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        Debug.Log("pathPtr: " + pathPtr.ToString());
        Debug.Log("pathLength: " + pathLength);

        if (pathPtr == IntPtr.Zero || pathLength == 0)
        {
            return null;
        }

        int[] pathArray = new int[pathLength * 2];

        Marshal.Copy(pathPtr, pathArray, 0, pathLength * 2);

        Marshal.FreeCoTaskMem(pathPtr);
        List<Vector2Int> path = new List<Vector2Int>(pathLength);

        for (int i = 0; i < pathLength; i++)
        {
            path.Add(new Vector2Int(pathArray[i * 2], pathArray[i * 2 + 1]));

            Debug.Log(new Vector2Int(pathArray[i * 2], pathArray[i * 2 + 1]));
        }

        return path;

    }
}
