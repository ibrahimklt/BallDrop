using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;         // Zıplama kuvvetini artırdık
    [SerializeField] private float horizontalForce = 3f;  // Yatay hareket için kuvvet
    [SerializeField] private float bounceMultiplier = 0.5f;
    [SerializeField] private float maxBounceForce = 7f;
    [SerializeField] private float minBounceForce = 2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGameStarted = false;
    private Vector2 surfaceNormal = Vector2.up;
    private bool canJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Başlangıçta yerçekimini sıfırla
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // Tüm hareketi dondur
    }

    void Update()
    {
        // Kamera kontrolünü daha hassas yapalım
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.y < -0.1f || viewPos.y > 1.1f || viewPos.x < -0.1f || viewPos.x > 1.1f)
        {
            // Debug.Log("Top kamera dışına çıktı: " + viewPos); // Test için
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return; // Diğer kontrolleri yapmaya gerek yok
        }

        // Debug.Log("IsGrounded: " + isGrounded); // Test için

        // Oyunu başlatma kontrolü
        if (!isGameStarted && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            isGameStarted = true;
            rb.gravityScale = 1.5f; // Orijinal yerçekimi değeri
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Sadece dönmeyi engelle
            if (isGrounded) canJump = true;
        }

        if (!isGameStarted) return;

#if UNITY_EDITOR
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && isGrounded && canJump)
        {
            Jump();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && isGrounded && canJump)
            {
                Jump();
            }
        }
#endif
    }

    private void Jump()
    {
        rb.linearVelocity = Vector2.zero;
        Vector2 jumpDirection = (Vector2.up + surfaceNormal).normalized;
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("Platform") ||
            collision.gameObject.CompareTag("TiltPlatform"))
        {
            isGrounded = true;

            // Temas yüzeyinin normalini kaydet
            if (collision.contactCount > 0)
            {
                surfaceNormal = collision.GetContact(0).normal;
            }

            // Sadece oyun başladıysa zıplamaya izin ver
            if (isGameStarted)
            {
                canJump = true;
            }

            // Otomatik sekme yok, sadece hızı sıfırla
            rb.linearVelocity = Vector2.zero;
        }
    }
}