using UnityEngine;

public class Node
{
    public Vector2Int Position { get; }
    public Node Parent { get; set; }
    public float GCost { get; set; }
    public float HCost { get; }
    public float FCost => GCost + HCost;

    public Node(Vector2Int pos, Node parent, float g, float h)
    {
        Position = pos;
        Parent = parent;
        GCost = g;
        HCost = h;
    }
}
