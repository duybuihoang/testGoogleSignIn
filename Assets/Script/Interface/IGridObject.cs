using UnityEngine;

public interface IGridObject
{
    void OnGridPositionChanged(int x, int y);
    bool CanMove(int targetX, int targetY);
}
