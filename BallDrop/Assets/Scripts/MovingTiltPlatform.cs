using UnityEngine;

public class MovingTiltPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;        // İlerleme hızı
    [SerializeField] private float moveDistance = 5f;     // Ne kadar ilerleyeceği
    [SerializeField] private float jumpForce = 7f;        // Zıplama kuvveti
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
    private bool isGrounded = true;  // canJump yerine isGrounded kullanacağız
    private Vector3 targetPosition;
    private BoxCollider2D platformCollider;

    void Start()
    {
        initialX = transform.position.x;
        targetPosition = transform.position;
        platformCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Zıplama kontrolü - isGrounded kontrolü yapıyoruz
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
            // Hedef pozisyonu hesapla
            targetPosition.x = transform.position.x + (moveSpeed * Time.fixedDeltaTime);
            float totalMoved = Mathf.Abs(targetPosition.x - initialX);

            if (totalMoved >= moveDistance)
            {
                isMoving = false;
                isRotating = true;
                targetPosition.x = initialX + moveDistance;
            }

            // Platform'u hareket ettir
            transform.position = targetPosition;

            if (playerRb != null)
            {
                if (isGrounded)
                {
                    // Yerdeyken platform ile birlikte hareket
                    playerTransform.position = targetPosition + playerOffset;
                    playerRb.linearVelocity = Vector2.zero;
                }
                else
                {
                    // Havadayken platform ile aynı hızda sağa git
                    Vector2 currentVelocity = playerRb.linearVelocity;
                    playerRb.linearVelocity = new Vector2(moveSpeed, currentVelocity.y);
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

            // Sağa doğru eğilme
            transform.Rotate(Vector3.forward, -step);

            // Platform eğilmeye başladığında
            if (playerRb != null && isGrounded)
            {
                // Top'u platformla birlikte döndür
                playerTransform.RotateAround(transform.position, Vector3.forward, -step);

                // Eğime göre hız ver
                float angle = transform.rotation.eulerAngles.z;
                if (angle > 180) angle -= 360; // -180 ile 180 arası açıya dönüştür

                // Eğim açısına göre kayma hızı
                float slideSpeed = Mathf.Abs(angle) * 0.1f;
                playerRb.linearVelocity = new Vector2(slideSpeed, -slideSpeed);
            }
        }
    }

    private void Jump()
    {
        playerRb.gravityScale = 1.5f;
        // Sadece yukarı yönde zıplama kuvveti uygula
        playerRb.linearVelocity = new Vector2(moveSpeed, jumpForce);
        isGrounded = false;  // Zıpladığında yerde değil
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // İlk kez değiyorsa hareketi başlat
            if (!isTriggered)
            {
                isTriggered = true;
            }

            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerTransform = collision.transform;
            playerOffset = collision.transform.position - transform.position;

            // Top'u anında platforma hizala
            playerTransform.position = transform.position + playerOffset;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.gravityScale = 0;
            isGrounded = true;  // Platforma değdiğinde yerde
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isGrounded = false;
            playerRb.gravityScale = 1.5f;  // Top'un yerçekimini aktif et
            playerRb = null;  // Referansları temizle
            playerTransform = null;
        }
    }
}