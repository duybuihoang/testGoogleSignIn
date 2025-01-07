using UnityEngine;

public class QueueingState : INPCState
{
    private readonly NPC npc;

    public QueueingState(NPC npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        WaitingQueue.Instance.AddToQueue(npc);
    }

    public void Exit()
    {
        WaitingQueue.Instance.RemoveFromQueue();

    }

    public void Update()
    {
    }
}
