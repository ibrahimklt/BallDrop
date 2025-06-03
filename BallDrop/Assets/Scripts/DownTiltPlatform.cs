using UnityEngine;

public class DownTiltPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;        // A�a�� inme h�z�
    [SerializeField] private float rotationSpeed = 90f;   // D�nme h�z�
    [SerializeField] private float moveDistance = 2f;     // Ne kadar a�a�� inece�i
    [SerializeField] private float maxRotation = 45f;     // Maximum d�n�� a��s�

    private bool isTriggered = false;
    private bool isMovingDown = true;    // �lk a�ama: a�a�� inme
    private bool isRotating = false;     // �kinci a�ama: d�nme
    private float initialY;              // Ba�lang�� Y pozisyonu
    private float currentRotation = 0f;
    private Vector2 pivotPoint;          // D�nme noktas�
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        initialY = transform.position.y;
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        pivotPoint = (Vector2)transform.position + new Vector2(-width / 2, 0); // Sol k��e
    }

    void FixedUpdate()
    {
        if (!isTriggered) return;

        if (isMovingDown)
        {
            // A�a�� hareket
            float newY = transform.position.y - (moveSpeed * Time.fixedDeltaTime);
            float totalMoved = initialY - newY;

            if (totalMoved >= moveDistance)
            {
                // Hareket tamamland�, d�nmeye ba�la
                isMovingDown = false;
                isRotating = true;
                // Pivot noktas�n� g�ncelle
                float width = GetComponent<SpriteRenderer>().bounds.size.x;
                pivotPoint = (Vector2)transform.position + new Vector2(-width / 2, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }
        else if (isRotating && currentRotation < maxRotation)
        {
            // Sa� k��eyi a�a�� d�nd�r
            float step = rotationSpeed * Time.fixedDeltaTime;
            currentRotation += step;

            if (currentRotation > maxRotation)
            {
                step -= (currentRotation - maxRotation);
                currentRotation = maxRotation;
            }

            // D�n�� y�n�n� de�i�tirdik (-step yapt�k)
            transform.RotateAround(pivotPoint, Vector3.forward, -step);
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