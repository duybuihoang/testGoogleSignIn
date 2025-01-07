using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : IPathFinder
{
    private Grid<IGridObject> grid;
    private const float DIAGONAL_COST = 1.414f; 
    private const float OBSTACLE_PENALTY = 10f;  // Penalty for moving near obstacles
    private bool[,] isWalkableArray;

    public AStarPathfinder(Grid<IGridObject> grid)
    {
        this.grid = grid;
        isWalkableArray = null; 
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        GetAllGrid();

        var open = new List<Node>();
        var closed = new HashSet<Vector2Int>();
        var nodes = new Dictionary<Vector2Int, Node>();

        var startNode = new Node(start, null, 0, GetHeuristic(start, end));
        open.Add(startNode);
        nodes[start] = startNode;

        while(open.Count > 0)
        {
            var current = GetLowestFCost(open);
            open.Remove(current);
            closed.Add(current.Position);

            if(current.Position == end)
            {
                return ReconstructPath(current);
            }

            foreach (var neighbor in GridUtils.GetNeighbors(current.Position.x, current.Position.y, true))
            {
                if(closed.Contains(neighbor) || !IsWalkable(neighbor, end))
                {
                    continue;
                }

                var gCost = current.GCost + 1;
                Node neighborNode;

                if (!nodes.TryGetValue(neighbor, out neighborNode))
                {
                    neighborNode = new Node(neighbor, current, gCost, GetHeuristic(neighbor, end));
                    nodes[neighbor] = neighborNode;
                    open.Add(neighborNode);
                }
                else if (gCost < neighborNode.GCost)
                {
                    neighborNode.Parent = current;
                    neighborNode.GCost = gCost;
                }
            }

        }
        return null;

    }

    private Node GetLowestFCost(List<Node> nodes)
    {
        var lowest = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].FCost < lowest.FCost) lowest = nodes[i];
        }
        return lowest;
    }

    private bool IsWalkable(Vector2Int pos, Vector2Int end)
    {

        Debug.Log(grid.GetValue(pos.x, pos.y));
        return GridUtils.IsWithinBounds(pos.x, pos.y,
           GridManager.Instance.GetGridWidth(),
           GridManager.Instance.GetGridHeight());
    }





     


    private void GetAllGrid()
    {

        Debug.Log("position");
        for (int i = 0; i < GridManager.Instance.GetGridHeight(); i++)
        {
            string x = $"";
            for (int j = 0; j < GridManager.Instance.GetGridWidth(); j++)
            {
                x += $"({i},{j}) ";
            }
            Debug.Log(x);
        }


        Debug.Log("value");
        for (int i = 0; i < GridManager.Instance.GetGridHeight(); i++)
        {
            string x = $"";
            for (int j = 0; j < GridManager.Instance.GetGridWidth(); j++)
            {
                x += $"({(grid.GetValue(i,j) == null ? 0 : 1)}) ";
            }
            Debug.Log(x);
        }
    }
    private List<Vector2Int> ReconstructPath(Node endNode)
    {
        var path = new List<Vector2Int>();
        var current = endNode;
        while (current != null)
        {
            path.Add(current.Position);
            current = current.Parent;
        }
        path.Reverse();
        foreach (var item in path)
        {
            Debug.Log(item);
        }

        return path;
    }
    private float GetHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }   
}
