using UnityEngine;

public class WaitingInQueueState : INPCState
{
    private readonly NPC npc;

    public WaitingInQueueState(NPC npc)
    {
        this.npc = npc;
    }
    public void Enter() 
    {
        npc.SetAnimation("Walk", false);
    }
    public void Exit() { }
    public void Update() { }
}
