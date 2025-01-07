using System.Collections;
using UnityEngine;

public class PulseScale : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float pulseDuration = 0.8f;

    private Vector3 originalScale;
    private Coroutine pulseCoroutine;


    private void Start()
    {
        originalScale = transform.localScale;
        StartPulse();
    }

    public void StartPulse()
    {
        StopPulse();
        pulseCoroutine = StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine()
    {
        while (true)
        {
            /*float elapsed = 0f;
            while (elapsed < pulseDuration / 2)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / (pulseDuration / 2);
                float currentScale = Mathf.Lerp(1f, maxScale, progress);
                transform.localScale = originalScale * currentScale;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < pulseDuration / 2)
            {
                elapsed += Time.deltaTime;
                Debug.Log(elapsed);
                float progress = elapsed / (pulseDuration / 2);
                float currentScale = Mathf.Lerp(maxScale, minScale, progress);
                transform.localScale = originalScale * currentScale;
                yield return null;
            }*/


            float elapsed = 0f;
            while (elapsed < 0.1f) 
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / (0.1f);
                float currentScale = Mathf.Lerp(1f, maxScale, progress);
                transform.localScale = originalScale * currentScale;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < 0.1f) 
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / (0.1f);
                float currentScale = Mathf.Lerp(maxScale, minScale, progress);
                transform.localScale = originalScale * currentScale;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < 0.1f) 
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / (0.1f);
                float currentScale = Mathf.Lerp(1f, maxScale, progress);
                transform.localScale = originalScale * currentScale;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < 0.1f) 
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / (0.1f);
                float currentScale = Mathf.Lerp(maxScale, 1, progress);
                transform.localScale = originalScale * currentScale;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < pulseDuration)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

        }
    }

    public void StopPulse()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }
        transform.localScale = originalScale;
    }

    private void OnDisable()
    {
        StopPulse();
    }


}
