using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScaleEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private float scaleMultiplier = 1.1f; 
    [SerializeField] private float scaleDuration = 0.1f; 
    private Button button;
    private Vector3 originalScale;
    private bool isPressed;

    private void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;

        isPressed = true;
        StartCoroutine(ScaleButton(originalScale * scaleMultiplier));

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPressed) return;

        isPressed = false;
        StartCoroutine(ScaleButton(originalScale));

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPressed) return;

        isPressed = false;
        StartCoroutine(ScaleButton(originalScale * scaleMultiplier));
    }

    private IEnumerator ScaleButton(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / scaleDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
