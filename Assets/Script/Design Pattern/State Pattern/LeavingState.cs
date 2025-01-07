using System;
using UnityEngine;


public class LeavingState : INPCState
{
    private readonly NPC npc;
    private readonly IMovementStrategy movement;
    private readonly Vector2Int ExitPosition;
    private readonly System.Action onPositionReached;

    public LeavingState(NPC npc, Vector2Int ExitPosition, IMovementStrategy movement, Action onPositionReached)
    {
        this.npc = npc;
        this.ExitPosition = ExitPosition;
        this.movement = movement;
        this.onPositionReached = onPositionReached;
    }

    public void Enter()
    {
        npc.SetAnimation("Walk", true);

        movement.OnDestinationReached += OnReachedExit;
        movement.MoveTo(ExitPosition);
    }

    public void Exit()
    {
        movement.OnDestinationReached -= OnReachedExit;
    }

    public void Update()
    {
    }

    private void OnReachedExit()
    {
        Debug.Log("left");
        npc.RequestDestroy();
    }
}

