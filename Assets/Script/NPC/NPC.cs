using System.Collections;
using UnityEngine;

public class NPC : BaseGridObject
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector2Int popupOffset = new Vector2Int(1, 1);
    [SerializeField] private GameObject coinPrefab;

    private INPCState currentState;
    private IMovementStrategy movement;
    private static IPathFinder pathfinder;

    private Animator anim;

    public MenuItem orderedItem;

    protected override void Awake()
    {
        pathfinder = new AStarPathfinder(GridManager.Instance.GetGrid());
        movement = new GridMovementStrategy(this, pathfinder, moveSpeed);
        anim = GetComponent<Animator>();
        this.Initialize();
    }

    public void ChangeState(INPCState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual void Update()
    {
        currentState?.Update();
    }

    public void Initialize()
    {
        if (WaitingQueue.Instance.CanAddToQueue())
        {
            WaitingQueue.Instance.AddToQueue(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public MenuItem GetRandomOrder()
    {
        var unlockedItems = MenuManager.Instance.GetUnlockedItems();
        Debug.Log(unlockedItems);
        if (unlockedItems.Count > 0)
        {
            return unlockedItems[Random.Range(0, unlockedItems.Count)];
        }
        return default;
    }

    public void AssignTable(Table table)
    {
        Debug.Log("move to table state");
        ChangeState(new MovingToTableState(this, table, movement));
    }

    public void MoveToQueuePosition(Vector2Int queuePosition, System.Action onPositionReached)
    {
        ChangeState(new MovingToQueueState(this, queuePosition, movement, onPositionReached));
    }

    public void LeaveRestaurant(System.Action onPositionReached)
    {
        ChangeState(new LeavingState(this, RestaurantManager.Instance.ExitPosition, movement, onPositionReached));
    }    

    public void SetAnimation(string name, bool state)
    {
        anim.SetBool(name, state);
    }

    public void RequestDestroy()
    {
        Destroy(this.gameObject);
    }

    public void EmitCoin(Vector3 targetPos, float value)
    {
        var coin = Instantiate(coinPrefab, targetPos, Quaternion.identity);
        coin.transform.position = this.transform.position;
        coin.GetComponent<CoinDropper>().SetValue(value);
        coin.GetComponent<CoinDropper>().DropCoinToPosition(targetPos);
    }    

    public override bool CanMove(int targetX, int targetY) => true;

}
