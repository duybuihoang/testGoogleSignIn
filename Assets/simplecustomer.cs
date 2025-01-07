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
            Debug.Log($"{name} ?ang x? lý l??t c?a mình.");
            // Th?c hi?n hành ??ng c?a NPC (thêm logic ? ?ây)
            StartCoroutine(EndTurn());
        }
    }

    private IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(2f); // Th?i gian th?c hi?n hành ??ng
        isProcessing = false;
    }
}
