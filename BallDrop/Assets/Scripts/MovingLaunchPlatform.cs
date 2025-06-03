using UnityEngine;

public class MovingLaunchPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;        // İlerleme hızı
    [SerializeField] private float moveDistance = 3f;     // Ne kadar ilerleyeceği
    [SerializeField] private float launchForce = 10f;     // Fırlatma kuvveti
    [SerializeField] private float launchAngle = 45f;     // Fırlatma açısı
    [SerializeField] private float rotationSpeed = 90f;   // Dönme hızı
    [SerializeField] private float maxRotation = 45f;     // Maximum dönüş açısı

    private bool isTriggered = false;
    private bool isMoving = true;        // İlk aşama: ilerleme
    private bool isRotating = false;     // İkinci aşama: dönme
    private bool isLaunching = false;    // Son aşama: fırlatma
    private float initialX;              // Başlangıç X pozisyonu
    private float currentRotation = 0f;
    private Vector3 startPosition;
    private Rigidbody2D playerRb;
    private Transform playerTransform;
    private Vector3 playerOffset;
    private Vector2 pivotPoint;

    void Start()
    {
        startPosition = transform.position;
        initialX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (!isTriggered) return;

        if (isMoving)
        {
            // Sola doğru hareket
            float newX = transform.position.x - (moveSpeed * Time.fixedDeltaTime);
            float totalMoved = Mathf.Abs(newX - initialX);

            if (totalMoved >= moveDistance)
            {
                // Hareket tamamlandı, dönmeye başla
                isMoving = false;
                isRotating = true;

                // Pivot noktasını sağ köşeye ayarla
                float width = GetComponent<SpriteRenderer>().bounds.size.x;
                pivotPoint = (Vector2)transform.position + new Vector2(width / 2, 0);
            }
            else
            {
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                if (playerRb != null && playerTransform != null)
                {
                    playerTransform.position = transform.position + playerOffset;
                    playerRb.linearVelocity = Vector2.zero;
                }
            }
        }
        else if (isRotating)
        {
            // Sol köşeyi yukarı döndür
            float step = rotationSpeed * Time.fixedDeltaTime;
            currentRotation += step;

            if (currentRotation >= maxRotation)
            {
                step -= (currentRotation - maxRotation);
                currentRotation = maxRotation;
                isRotating = false;
                isLaunching = true;
            }

            transform.RotateAround(pivotPoint, Vector3.forward, -step);

            if (playerRb != null && playerTransform != null)
            {
                playerTransform.position = transform.position + playerOffset;
                playerRb.linearVelocity = Vector2.zero;
            }
        }
        else if (isLaunching && playerRb != null)
        {
            // Topu sağ-yukarı yönde fırlat
            Vector2 launchDirection = new Vector2(
                Mathf.Cos(launchAngle * Mathf.Deg2Rad),
                Mathf.Sin(launchAngle * Mathf.Deg2Rad)
            ).normalized;

            playerRb.linearVelocity = launchDirection * launchForce;
            isLaunching = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerTransform = collision.transform;
            playerOffset = collision.transform.position - transform.position;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.gravityScale = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb.gravityScale = 1.5f;
            playerRb = null;
            playerTransform = null;
        }
    }
}