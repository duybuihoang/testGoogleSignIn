using UnityEngine;

public interface IMovementStrategy 
{
    void MoveTo(Vector2Int target);
    bool IsMoving { get; }
    event System.Action OnDestinationReached;
}
