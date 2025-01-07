using UnityEngine;

public abstract class BaseGridObject : BaseMonobehavior, IGridObject
{
    public int currentX;
    public int currentY;
    protected Grid<IGridObject> grid;

    public virtual void Initialize(Grid<IGridObject> grid, int x, int y)
    {
        this.currentX = x;
        this.currentY = y;
        this.grid = grid;
    }

    public virtual void SetValue(int x, int y)
    {
        grid.SetValue(y, x, this);
    }
    public abstract bool CanMove(int targetX, int targetY);

    public virtual void OnGridPositionChanged(int x, int y)
    {
        transform.position = grid.GetWorldPosition(x, y);
    }
}
