using UnityEngine;
public class MovingToTableState : INPCState
{

    private readonly NPC npc;
    private readonly Table targetTable;
    private readonly IMovementStrategy movement;

    public MovingToTableState(NPC npc, Table targetTable, IMovementStrategy movement)
    {
        this.npc = npc;
        this.targetTable = targetTable;
        this.movement = movement;
    }

    public void Enter()
    {
        npc.SetAnimation("Walk", true);

        movement.OnDestinationReached += OnReachedTable;
        movement.MoveTo(new Vector2Int(targetTable.currentX, targetTable.currentY));
    }

    public void Exit()
    {
        movement.OnDestinationReached -= OnReachedTable;
    }

    public void Update()
    {
    }

    private void OnReachedTable()
    {
        npc.ChangeState(new StartOrderingState(npc, targetTable));
    }
}
