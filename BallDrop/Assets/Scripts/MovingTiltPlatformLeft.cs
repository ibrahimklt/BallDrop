using UnityEngine;

public class MovingTiltPlatformLeft : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float maxRotation = 30f;

    private bool isTriggered = false;
    private bool isMoving = true;
    private bool isRotating = false;
    private float initialX;
    private float currentRotation = 0f;
    private Rigidbody2D playerRb;
    private Transform playerTransform;
    private Vector3 playerOffset;
    private bool isGrounded = true;
    private Vector3 targetPosition;

    void Start()
    {
        initialX = transform.position.x;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (playerRb != null && isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (!isTriggered) return;

        if (isMoving)
        {
            // Sola do�ru hareket (moveSpeed'i eksi yap�yoruz)
            targetPosition.x = transform.position.x - (moveSpeed * Time.fixedDeltaTime);
            float totalMoved = Mathf.Abs(targetPosition.x - initialX);

            if (totalMoved >= moveDistance)
            {
                isMoving = false;
                isRotating = true;
                targetPosition.x = initialX - moveDistance; // Sola do�ru mesafe
            }

            transform.position = targetPosition;

            if (playerRb != null)
            {
                if (isGrounded)
                {
                    playerTransform.position = targetPosition + playerOffset;
                    playerRb.linearVelocity = Vector2.zero;
                }
                else
                {
                    Vector2 currentVelocity = playerRb.linearVelocity;
                    playerRb.linearVelocity = new Vector2(-moveSpeed, currentVelocity.y); // Sola do�ru h�z
                }
            }
        }
        else if (isRotating && currentRotation < maxRotation)
        {
            float step = rotationSpeed * Time.fixedDeltaTime;
            currentRotation += step;

            if (currentRotation > maxRotation)
            {
                step -= (currentRotation - maxRotation);
                currentRotation = maxRotation;
            }

            // Sola do�ru e�ilme (step'i eksi yapm�yoruz)
            transform.Rotate(Vector3.forward, step);

            if (playerRb != null && isGrounded)
            {
                // Top'u platformla birlikte d�nd�r
                playerTransform.RotateAround(transform.position, Vector3.forward, step);

                // E�ime g�re h�z ver
                float angle = transform.rotation.eulerAngles.z;
                if (angle > 180) angle -= 360;

                // E�im a��s�na g�re kayma h�z� (sola do�ru)
                float slideSpeed = Mathf.Abs(angle) * 0.1f;
                playerRb.linearVelocity = new Vector2(-slideSpeed, -slideSpeed);
            }
        }
    }

    private void Jump()
    {
        playerRb.gravityScale = 1.5f;
        playerRb.linearVelocity = new Vector2(-moveSpeed, jumpForce); // Sola do�ru z�plama
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isTriggered)
            {
                isTriggered = true;
            }

            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerTransform = collision.transform;
            playerOffset = collision.transform.position - transform.position;

            playerTransform.position = transform.position + playerOffset;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.gravityScale = 0;
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isGrounded = false;
        }
    }
}