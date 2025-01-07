using UnityEngine;


public class StartOrderingState : INPCState
{
    private readonly NPC npc;
    private readonly Table table;
    private GameObject currentOrderBox;
    private GameObject currentOrderPopup;

    [SerializeField] private Vector2Int popupOffset = new Vector2Int(1, 2);



    public StartOrderingState(NPC npc, Table table)
    {
        this.npc = npc;
        this.table = table;
    }

    public void Enter()
    {
        npc.SetAnimation("Walk", false);
        table.TryOccupyTable(npc);
        PopupOrder();

        //
    }

    public void Exit()
    {
        //table.ReleaseTable();
    }

    public void Update()
    {

    }

    public void PopupOrder()
    {
        // Instantiate popup above NPC's head
        Vector2Int popupPosition = GridManager.Instance.GetGrid().GetXY(npc.transform.position);
        MenuItem item = npc.GetRandomOrder();
        Debug.Log(npc);
        Debug.Log(item);


        currentOrderBox = FoodSpawner.Instance.SpawnObject(
            "ORDER",
            popupPosition.x + popupOffset.x,
            popupPosition.y + popupOffset.y, 
            npc.transform
            );

        currentOrderPopup = FoodSpawner.Instance.SpawnObject(
            "FOOD",
            popupPosition.x + popupOffset.x,
            popupPosition.y + popupOffset.y,
            currentOrderBox.transform
            );

        currentOrderPopup.GetComponent<Food>().SetItem(item);

        // Make popup clickable
        OrderPopup popupScript = currentOrderBox.GetComponent<OrderPopup>();
        popupScript.OnPopupClicked.AddListener(HandleOrderFulfilled);
    }
    private void HandleOrderFulfilled()
    {
        if (currentOrderBox != null)
        {
            currentOrderPopup.transform.SetParent(null);
            currentOrderBox.GetComponent<OrderPopup>().RequestDestroy(currentOrderBox);
        }
        //currentOrderPopup.transform.position = table.transform.position;
        npc.ChangeState(new EatingState(npc, table, currentOrderPopup, 8f));
    }

    
}
