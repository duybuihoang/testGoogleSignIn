using UnityEngine;

public class EatingState : INPCState
{
    private readonly NPC npc;
    private readonly Table table;
    private float remainingTime;
    private GameObject currentFood;

    public EatingState(NPC npc, Table table,GameObject food, float remainingTime)
    {
        this.npc = npc;
        this.table = table;
        this.remainingTime = remainingTime;
        this.currentFood = food;
    }

    public void Enter()
    {
        //spawn food on table
        
        currentFood.transform.position = table.transform.position;
    }

    public void Exit()
    {
        //remove food on table
        table.ReleaseTable();


        var tableCell = GridManager.Instance.GetGrid().GetXY(table.transform.position);
        int x = tableCell.x + 2;
        int y = tableCell.y + 1;

        float randomX = Random.Range(-.15f, .15f);
        float randomy = Random.Range(-.15f, .15f);


        
        npc.EmitCoin(GridManager.Instance.GetGrid().GetWorldPosition(x, y) + new Vector3(randomX, randomy), 
            currentFood.GetComponent<Food>().Item.basePrice + table.CurrentIncome);
        //npc.EmitCoin(GetRandomNeighborPos());

    }

    public void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            Debug.Log("Finish eating");
            currentFood.GetComponent<Food>().RequestDestroy(currentFood);
            npc.LeaveRestaurant(null);
        }
    }

}
