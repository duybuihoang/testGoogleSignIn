using System.Collections;
using UnityEngine;

public class simplecustomer : MonoBehaviour
{
    public float moveDuration = 2f; 
    private bool isProcessing = false;
    public void MoveTo(Vector3 targetPosition, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(targetPosition, speed));
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void ProcessTurn()
    {
        if (!isProcessing)
        {
            isProcessing = true;
            Debug.Log($"{name} ?ang x? l� l??t c?a m�nh.");
            // Th?c hi?n h�nh ??ng c?a NPC (th�m logic ? ?�y)
            StartCoroutine(EndTurn());
        }
    }

    private IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(2f); // Th?i gian th?c hi?n h�nh ??ng
        isProcessing = false;
    }
}
