using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    public Vector2 GetMouseGridPosition()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return GridManager.Instance.GetGrid().GetXY(mouseWorldPosition);
    }


}
