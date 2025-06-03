using UnityEngine;

public class TiltPlatform : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float maxRotation = 45f;

    private bool isTriggered = false;
    private float currentRotation = 0f;
    private Vector2 pivotPoint;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Pivot noktasýný platformun sað ucuna ayarla
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        pivotPoint = (Vector2)transform.position + new Vector2(width / 2, 0);
    }

    void FixedUpdate()
    {
        if (isTriggered && currentRotation < maxRotation)
        {
            // Dönüþ miktarýný hesapla
            float step = rotationSpeed * Time.fixedDeltaTime;
            currentRotation += step;

            // Dönüþü sýnýrla
            if (currentRotation > maxRotation)
            {
                step -= (currentRotation - maxRotation);
                currentRotation = maxRotation;
            }

            // Platformu sað ucundan döndür, sol ucu aþaðý inecek
            transform.RotateAround(
                pivotPoint,
                Vector3.forward,
                step
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
}