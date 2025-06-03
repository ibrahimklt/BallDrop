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
        // Pivot noktas�n� platformun sol veya sa� ucuna ayarla
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        pivotPoint = (Vector2)transform.position + new Vector2(rotateFromLeft ? -width / 2 : width / 2, 0);
    }

    void FixedUpdate()
    {
        if (isTriggered && currentRotation < maxRotation)
        {
            // D�n�� miktar�n� hesapla
            float step = rotationSpeed * Time.fixedDeltaTime;
            currentRotation += step;

            // D�n��� s�n�rla
            if (currentRotation > maxRotation)
            {
                step -= (currentRotation - maxRotation);
                currentRotation = maxRotation;
            }

            // Platformu d�nd�r (d�n�� y�n�n� tersine �evirdik)
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