using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitingQueue : BaseMonobehavior
{
    private class NPCQueueData
    {
        public NPC NPC { get; set; }
        public int TargetQueueIndex { get; set; }
        public bool IsMovingToPosition { get; set; }
    }

    [SerializeField] private int maxQueueLength = 10;
    [SerializeField] private float spaceBetweenNPCs = 1f;

    private List<NPCQueueData> queuedNPCs = new List<NPCQueueData>();
    private Vector3 queueStartPosition;

    private static WaitingQueue instance;
    public static WaitingQueue Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {
        // Set queue position relative to grid
        queueStartPosition = transform.position;
    }

    public bool CanAddToQueue() => queuedNPCs.Count < maxQueueLength;
    public void AddToQueue(NPC npc)
    {
        if (!CanAddToQueue()) return;

        var queueData = new NPCQueueData
        {
            NPC = npc,
            TargetQueueIndex = queuedNPCs.Count,
            IsMovingToPosition = true
        };

        queuedNPCs.Add(queueData);
        AssignQueuePosition(queueData);
    }
    public void RemoveFromQueue()
    {
        /*  if (queuedNPCs.Count == 0) return null;

          NPC firstInLine = queuedNPCs[0].NPC;
          queuedNPCs.RemoveAt(0);
  */

        if (queuedNPCs.Count == 0) return;
        queuedNPCs.RemoveAt(0);

        // Update queue positions for remaining NPCs
        for (int i = 0; i < queuedNPCs.Count; i++)
        {
            queuedNPCs[i].TargetQueueIndex = i;
            AssignQueuePosition(queuedNPCs[i]);
        }

/*        return firstInLine;
*/    }

    public NPC PopFromQueue()
    {
        if (queuedNPCs.Count == 0) return null;
        return queuedNPCs[0].NPC;
    }    

    private void AssignQueuePosition(NPCQueueData queueData)
    {
        Vector2Int targetGridPos = GetGridPositionForQueueIndex(queueData.TargetQueueIndex);
        queueData.NPC.MoveToQueuePosition(targetGridPos, () =>
        {
            queueData.IsMovingToPosition = false;
        });
    }

    private Vector2Int GetGridPositionForQueueIndex(int index)
    {
        Vector3 worldPos = queueStartPosition + new Vector3(0, index * spaceBetweenNPCs, 0);
        return GridManager.Instance.GetGrid().GetXY(worldPos);
    }

    public bool IsNPCReadyInQueue(NPC npc)
    {
        var queueData = queuedNPCs.FirstOrDefault(q => q.NPC == npc);
        return queueData != null && !queueData.IsMovingToPosition;
    }
}
