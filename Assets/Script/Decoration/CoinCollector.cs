using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private float radius;   
    
    void Update()
    {
        if (Input.touchCount > 0) // Check if there is at least one touch
        {
            Touch touch = Input.GetTouch(0); // Get the first touch
            if (touch.phase == TouchPhase.Began) // Check if the touch just started
            {
                Vector3 center = Camera.main.ScreenToWorldPoint(touch.position);
                center.z = 0; // Ensure the z-coordinate is 0 for 2D world space
                CollectCoin(center);
            }
        }
    }

    private void CollectCoin(Vector2 center)
    {
        var colliders = Physics2D.OverlapCircleAll(center, radius);

        foreach (var item in colliders)
        {
            if(item.TryGetComponent<CoinDropper>(out CoinDropper coin))
            {
                coin.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(coin.FlyToUI());
                AudioManager.Instance.PlaySFX("coin-collect-retro-8-bit-sound-effect-145251 (1)");
            }    
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
