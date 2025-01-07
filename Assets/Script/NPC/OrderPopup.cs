using UnityEngine;

public class OrderPopup : BaseGridObject
{
    public UnityEngine.Events.UnityEvent OnPopupClicked;



    private void OnMouseDown()
    {
        OnPopupClicked?.Invoke();
        AudioManager.Instance.PlaySFX("90s-game-ui-7-185100");  
    }

    private void Start()
    {
        gameObject.AddComponent<PulseScale>();
    }


    public void RequestDestroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public override bool CanMove(int targetX, int targetY) => false;
}
