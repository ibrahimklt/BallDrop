using UnityEngine;

public class SlidePlatform : MonoBehaviour
{
    [SerializeField] private float slideMultiplier = 0.3f;
    [SerializeField] private float jumpForce = 10f;        // Zıplama kuvvetini artırdık
    [SerializeField] private float bounceMultiplier = 2f;  // Ek hızı artırdık

    private Rigidbody2D playerRb;
    private bool isGrounded = false;

    void Update()
    {
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (isGrounded && playerRb != null)
        {
            // Platformun açısını al
            float angle = transform.rotation.eulerAngles.z;
            if (angle > 180) angle -= 360;

            // Eğime göre kayma hızı
            float slideSpeed = Mathf.Abs(angle) * slideMultiplier;
            float direction = angle > 0 ? -1 : 1;

            // Kayma hızını uygula
            playerRb.linearVelocity = new Vector2(slideSpeed * direction, -slideSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            isGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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

    private void Jump()
    {
        if (playerRb != null)
        {
            // Platformun açısını al
            float angle = transform.rotation.eulerAngles.z;
            if (angle > 180) angle -= 360;

            // Eğim yönünde zıplama
            float direction = angle > 0 ? -1 : 1;

            // Yukarı ve yana doğru kuvvet uygula
            Vector2 jumpVelocity = new Vector2(
                direction * bounceMultiplier, // Yana doğru kuvvet
                jumpForce                     // Yukarı doğru kuvvet
            );

            playerRb.linearVelocity = jumpVelocity;
            isGrounded = false;
        }
    }
}