using UnityEngine;

public class CircleRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;    // Dönüş hızı
    [SerializeField] private float jumpForce = 7f;         // Zıplama kuvveti
    [SerializeField] private bool rotateClockwise = true;  // Saat yönünde dönüş

    private Rigidbody2D playerRb;
    private Transform playerTransform;
    private Vector3 playerOffset;
    private bool isGrounded = false;
    private float initialPlayerX; // Top'un başlangıç X pozisyonu

    void FixedUpdate()
    {
        // Platform dönüşü
        float direction = rotateClockwise ? -1 : 1;
        transform.Rotate(Vector3.forward * direction * rotationSpeed * Time.fixedDeltaTime);

        // Top platformda ise
        if (playerRb != null && isGrounded)
        {
            // Top'un pozisyonunu güncelle, X'i sabit tut
            Vector3 newPosition = transform.position + playerOffset;
            newPosition.x = initialPlayerX;
            playerTransform.position = newPosition;

            // Platformdayken X hızını sıfırla
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
            playerRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        // Zıplama kontrolü
        if (isGrounded && playerRb != null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Jump();
        }
    }

    void Jump()
    {
        // X hızını sıfırlayarak sadece yukarı zıplat
        playerRb.linearVelocity = new Vector2(0, jumpForce);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerTransform = collision.transform;
            playerOffset = collision.transform.position - transform.position;
            initialPlayerX = collision.transform.position.x;

            // X hareketini dondur
            playerRb.linearVelocity = Vector2.zero;
            playerRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            playerRb.gravityScale = 0;
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // X hareketini serbest bırak
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRb.gravityScale = 1.5f;
            isGrounded = false;
        }
    }
}