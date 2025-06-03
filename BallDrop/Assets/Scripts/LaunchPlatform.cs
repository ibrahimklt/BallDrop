using UnityEngine;

public class LaunchPlatform : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;    // Dönme hızı
    [SerializeField] private float maxRotation = 45f;      // Maximum dönüş açısı
    [SerializeField] private float launchForce = 10f;      // Fırlatma kuvveti

    private bool isTriggered = false;
    private float currentRotation = 0f;
    private Vector2 pivotPoint;
    private Rigidbody2D playerRb;

    void Start()
    {
        // Pivot noktasını platformun sağ ucuna ayarla
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        pivotPoint = (Vector2)transform.position + new Vector2(width / 2, 0);
    }

    void FixedUpdate()
    {
        if (isTriggered && currentRotation < maxRotation)
        {
            // Dönüş miktarını hesapla
            float step = rotationSpeed * Time.fixedDeltaTime;
            currentRotation += step;

            // Dönüşü sınırla
            if (currentRotation > maxRotation)
            {
                step -= (currentRotation - maxRotation);
                currentRotation = maxRotation;
            }

            // Platformu sağ ucundan döndür, sol ucu yukarı kalkacak
            transform.RotateAround(
                pivotPoint,
                Vector3.forward,
                -step
            );

            // Eğer top hala platformun üzerindeyse, onu da hareket ettir
            if (playerRb != null)
            {
                // Sağ-yukarı yönde fırlatma (45 derece)
                Vector2 launchDirection = new Vector2(
                    Mathf.Cos(45 * Mathf.Deg2Rad),  // Sağ için +x (eksi işaretini kaldırdık)
                    Mathf.Sin(45 * Mathf.Deg2Rad)    // Yukarı için y
                ).normalized;

                // Top'u fırlat
                playerRb.linearVelocity = launchDirection * launchForce;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = null;
        }
    }
}