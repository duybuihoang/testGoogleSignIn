using UnityEngine;
using UnityEngine.UI;
using Firebase.Crashlytics;
public class UISlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private int maxClickTime = 8;
    private int currentClickTime;

    private Button button;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>(); 
        currentClickTime = 0;
        slider.gameObject.SetActive(false);
        button = GetComponent<Button>();

    }

    private void Start()
    {
        button.gameObject.AddComponent<ButtonScaleEffect>();
    }
    public void Onclick()
    {
        AudioManager.Instance.PlaySFX("clonck");

        currentClickTime = (currentClickTime + 1 )%maxClickTime;

        Debug.Log(currentClickTime);
        slider.gameObject.SetActive(true);

        Analytics.Instance.LogCustomEvent("button", "guest");
        
        if(currentClickTime != 0)
        {
            slider.value = (float)currentClickTime / maxClickTime;
        }    
        else
        {
            slider.gameObject.SetActive(false);

            if (WaitingQueue.Instance.CanAddToQueue())
            {
                GameObject npcObj = NPCSpawner.Instance.SpawnObject("NPC", GridManager.Instance.GetGridWidth() - 3, GridManager.Instance.GetGridHeight() - 1);

                NPC npc = npcObj.GetComponent<NPC>();
            }
        }
    }    
}
