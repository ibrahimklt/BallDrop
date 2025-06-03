using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float maxRotation = 45f;
    [SerializeField] private bool rotateFromLeft = true;

    private bool isTriggered = false;
    private float currentRotation = 0f;
    private Vector2 pivotPoint;

    void Start()
    {
        // Pivot noktasýný platformun sol veya sað ucuna ayarla
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        pivotPoint = (Vector2)transform.position + new Vector2(rotateFromLeft ? -width / 2 : width / 2, 0);
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

            // Platformu döndür (dönüþ yönünü tersine çevirdik)
            transform.RotateAround(
                pivotPoint,
                Vector3.forward,
                rotateFromLeft ? -step : step
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }
}