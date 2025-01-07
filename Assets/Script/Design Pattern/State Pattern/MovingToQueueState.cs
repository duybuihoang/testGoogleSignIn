using System;
using UnityEngine;

public class MovingToQueueState : INPCState
{
    private readonly NPC npc;
    private readonly Vector2Int queuePosition;
    private readonly IMovementStrategy movement;
    private readonly System.Action onPositionReached;

    public MovingToQueueState(NPC npc, Vector2Int queuePosition, IMovementStrategy movement, Action onPositionReached)
    {
        this.npc = npc;
        this.queuePosition = queuePosition;
        this.movement = movement;
        this.onPositionReached = onPositionReached;
    }

    public void Enter()
    {
        Debug.Log("move to queue");
        npc.SetAnimation("Walk", true);

        movement.OnDestinationReached += OnReachedQueuePosition;       
        movement.MoveTo(queuePosition);
    }

    public void Exit()
    {
        movement.OnDestinationReached -= OnReachedQueuePosition;
    }

    public void Update()
    {
    }

    private void OnReachedQueuePosition()
    {
        onPositionReached?.Invoke();
        npc.ChangeState(new WaitingInQueueState(npc));
    }
}
