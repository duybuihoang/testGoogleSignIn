using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : BaseGridObject
{
    private Canvas canvas;
    private TextMeshProUGUI text;
    [SerializeField] 
    private float fadeOutTime = .5f;
    [SerializeField]
    private float moveSpeed = 10f;

    protected override void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        Camera camera = Camera.main;
        SetUpCanvas(camera);
    }

    private void SetUpCanvas(Camera camera)
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = camera;
        canvas.sortingLayerName = "UI";

    }


    public IEnumerator AnimateDamageNumber(string numberText, Vector2 offset)
    {
        float elapsedTime = 0f;
        Vector2 startPos = transform.position;
        Color startColor = text.color;
        text.text = numberText;
        text.rectTransform.position = transform.position;

        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeOutTime;

            // Move up
            transform.position = startPos + (Vector2.up + offset) * moveSpeed * t;

            // Fade out
            text.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                Mathf.Lerp(1f, 0f, t)
            );

            yield return null;
        }

        Destroy(transform.gameObject);
    }

    void Update()
    {
        
    }

    public override bool CanMove(int targetX, int targetY) => false;
}
