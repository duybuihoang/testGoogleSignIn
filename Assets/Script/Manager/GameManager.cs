using UnityEngine;

public class GameManager : BaseMonobehavior
{

    [SerializeField] private float maxTime = 20f;
    [SerializeField] private float minTime = 10f;

    private float currentRandomTime;
    private float currentTime;

    protected override void Awake()
    {
        base.Awake();
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        StartCoroutine(AudioManager.Instance.PlayRandomBGM(0.5f));
        SetRandomTime();
        currentTime = Time.time;




    }

    private void SetRandomTime()
    {
        currentRandomTime =  Random.Range(minTime, maxTime);
    }    

    private void Update()
    {

        if(Time.time >= currentTime + currentRandomTime)
        {
            currentTime = Time.time; 
            SetRandomTime();

            if (WaitingQueue.Instance.CanAddToQueue())
            {
                GameObject npcObj = NPCSpawner.Instance.SpawnObject("NPC", GridManager.Instance.GetGridWidth() - 3, GridManager.Instance.GetGridHeight() - 1);

                NPC npc = npcObj.GetComponent<NPC>();
            }


        }
        for (int i = 0; i < GridManager.Instance.GetGridWidth(); i++)
        {
            for (int j = 0; j < GridManager.Instance.GetGridHeight(); j++)
            {
                Debug.DrawLine(GridManager.Instance.GetGrid().GetWorldPosition(i, j), GridManager.Instance.GetGrid().GetWorldPosition(i, j + 1), Color.blue);
                Debug.DrawLine(GridManager.Instance.GetGrid().GetWorldPosition(i, j), GridManager.Instance.GetGrid().GetWorldPosition(i + 1, j), Color.blue);

            }
            
        }
    }
}
