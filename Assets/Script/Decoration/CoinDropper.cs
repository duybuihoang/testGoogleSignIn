using System.Collections;
using UnityEngine;

public class CoinDropper : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private float initialVelocityY = 5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float dropDuration = 1f;

    [Header("Bounce Settings")]
    [SerializeField] private float initialBounceHeight = 1.5f;  
    [SerializeField] private float bounceDecay = 0.6f;         
    [SerializeField] private int maxBounces = 2;              
    [SerializeField] private float bounceSpeedMultiplier = 1.2f;

    [Header("UI Settings")]
    private bool MovingToUI;
    [SerializeField] public RectTransform UITransform;
    [SerializeField] private float flySpeed = 1f;

    [Header("Coin Value")]
    private float value;

    private void Awake()
    {
        var x = GameObject.Find("TopLeft");
        UITransform = x.transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void SetValue(float amount)
    {
        value = amount;
    }    

    public void DropCoinToPosition(Vector2 targetPosition)
    {
        StartCoroutine(DropCoinWithParabolicMotion(targetPosition));
    }

/*    private void Start()
    {
        StartCoroutine(DropCoinWithParabolicMotion(new Vector2(1, 0)));
    }*/

    private IEnumerator DropCoinWithParabolicMotion(Vector2 targetPosition)
    {
        Vector2 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < dropDuration)
        {
            float t = elapsedTime / dropDuration;

            // Lerp for X movement
            float x = Mathf.Lerp(startPos.x, targetPosition.x, t);

            // Parabolic for Y movement
            float y = startPos.y + (initialVelocityY * t) - (0.5f * gravity * t * t);
            transform.position = new Vector2(x, y);
            transform.Rotate(0, 0, 360 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        //StartCoroutine(MultipleBounce());

    }


    private IEnumerator MultipleBounce()
    {
        Vector3 basePosition = transform.position;
        float currentBounceHeight = initialBounceHeight;

        for (int bounce = 0; bounce < maxBounces; bounce++)
        {
            float bounceDuration = Mathf.Sqrt(2 * currentBounceHeight / gravity) * bounceSpeedMultiplier;
            float elapsedTime = 0f;
            while (elapsedTime < bounceDuration / 2)
            {
                float t = elapsedTime / (bounceDuration / 2);
                float height = Mathf.Lerp(0, currentBounceHeight, Mathf.Sin(t * Mathf.PI / 2));
                transform.position = basePosition + new Vector3(0, height, 0);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            while (elapsedTime < bounceDuration)
            {
                float t = (elapsedTime - bounceDuration / 2) / (bounceDuration / 2);
                float height = Mathf.Lerp(currentBounceHeight, 0, Mathf.Sin(t * Mathf.PI / 2));
                transform.position = basePosition + new Vector3(0, height, 0);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Reduce bounce height for next iteration
            currentBounceHeight *= bounceDecay;

            // Add small pause between bounces
            yield return new WaitForSeconds(0.05f);

        }
        transform.position = basePosition;
    }

    public IEnumerator FlyToUI()
    {
        Vector3 targetPos = (UITransform.transform.position);

        Vector2Int xy = new Vector2Int(
            GridManager.Instance.GetGrid().GetXY(transform.position).x,
            GridManager.Instance.GetGrid().GetXY(transform.position).y
            );

        var text = FloatingTextSpawner.Instance.SpawnObject("TEXT",
            xy.x,
            xy.y
            );
        text.transform.position = transform.position;
        StartCoroutine(text.GetComponent<FloatingText>().AnimateDamageNumber("+" + value, Vector2.zero));

        Debug.Log(targetPos);

        while (Vector2.Distance(targetPos, transform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, flySpeed * Time.deltaTime);
            yield return null;
        }

        MoneyManager.Instance.AddMoney(value);
        Destroy(gameObject);
    }
}
