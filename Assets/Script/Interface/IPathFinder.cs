using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder
{
    List<Vector2Int> FindPath(Vector2Int start, Vector2Int end);
}
