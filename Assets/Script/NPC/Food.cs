using UnityEngine;

public class Food : BaseGridObject
{
    private SpriteRenderer currentSprite;
    protected MenuItem item;
    public MenuItem Item => item;
    public override bool CanMove(int targetX, int targetY) => false;
    public void RequestDestroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    protected override void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
    }

    public void SetItem(MenuItem item)
    {
        this.item = item;

        currentSprite.sprite = item.icon;
    }    
}
