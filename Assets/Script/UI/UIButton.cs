using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : BaseMonobehavior
{
    public Vector2Int spawnPoint;
    [SerializeField] private GameObject MenuUI;
    private Button button;
    private bool isPressed;


    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.gameObject.AddComponent<ButtonScaleEffect>();
    }

    public void UnlockMenu()
    {
        AudioManager.Instance.PlaySFX("clonck");

        MenuUI.SetActive(true);
    }

    public void UpgradeTable()
    {
        AudioManager.Instance.PlaySFX("clonck");

        RestaurantManager.Instance.Upgrade();
    }    
    public void LockMenu()
    {


        AudioManager.Instance.PlaySFX("clonck");

        MenuUI.SetActive(false);
    }
    private void Update()
    {
        //onclick();
    }
    public void TrySpawnNPC()
    {
        Debug.Log("try spawning");
        if (WaitingQueue.Instance.CanAddToQueue())
        {
            GameObject npcObj = NPCSpawner.Instance.SpawnObject("NPC", GridManager.Instance.GetGridWidth() - 3, GridManager.Instance.GetGridHeight() - 1);
            
            NPC npc = npcObj.GetComponent<NPC>();
        }
    }


}
