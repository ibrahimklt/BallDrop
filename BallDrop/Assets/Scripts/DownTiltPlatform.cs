using UnityEngine;

public class DownTiltPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;        // Aþaðý inme hýzý
    [SerializeField] private float rotationSpeed = 90f;   // Dönme hýzý
    [SerializeField] private float moveDistance = 2f;     // Ne kadar aþaðý ineceði
    [SerializeField] private float maxRotation = 45f;     // Maximum dönüþ açýsý

    private bool isTriggered = false;
    private bool isMovingDown = true;    // Ýlk aþama: aþaðý inme
    private bool isRotating = false;     // Ýkinci aþama: dönme
    private float initialY;              // Baþlangýç Y pozisyonu
    private float currentRotation = 0f;
    private Vector2 pivotPoint;          // Dönme noktasý
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        initialY = transform.position.y;
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        pivotPoint = (Vector2)transform.position + new Vector2(-width / 2, 0); // Sol köþe
    }

    void FixedUpdate()
    {
        if (!isTriggered) return;

        if (isMovingDown)
        {
            // Aþaðý hareket
            float newY = transform.position.y - (moveSpeed * Time.fixedDeltaTime);
            float totalMoved = initialY - newY;

            if (totalMoved >= moveDistance)
            {
                // Hareket tamamlandý, dönmeye baþla
                isMovingDown = false;
                isRotating = true;
                // Pivot noktasýný güncelle
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
            // Sað köþeyi aþaðý döndür
            float step = rotationSpeed * Time.fixedDeltaTime;
            currentRotation += step;

            if (currentRotation > maxRotation)
            {
                step -= (currentRotation - maxRotation);
                currentRotation = maxRotation;
            }

            // Dönüþ yönünü deðiþtirdik (-step yaptýk)
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