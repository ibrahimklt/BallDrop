using UnityEngine;

public class Spike : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCollider();
    }

    void OnValidate()
    {
        // Editor'de boyut de�i�ti�inde �al���r
        if (polygonCollider == null) polygonCollider = GetComponent<PolygonCollider2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        if (polygonCollider != null && spriteRenderer != null)
        {
            // Sprite'�n boyutlar�n� al
            Vector2 size = spriteRenderer.bounds.size;
            float width = size.x;
            float height = size.y;

            // ��gen collider'� sprite boyutuna g�re ayarla
            polygonCollider.points = new Vector2[]
            {
                new Vector2(-width/2, -height/2),  // Sol alt
                new Vector2(width/2, -height/2),   // Sa� alt
                new Vector2(0, height/2)           // �st orta
            };
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Oyunu yeniden ba�lat
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}