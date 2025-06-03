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
        // Editor'de boyut deðiþtiðinde çalýþýr
        if (polygonCollider == null) polygonCollider = GetComponent<PolygonCollider2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        if (polygonCollider != null && spriteRenderer != null)
        {
            // Sprite'ýn boyutlarýný al
            Vector2 size = spriteRenderer.bounds.size;
            float width = size.x;
            float height = size.y;

            // Üçgen collider'ý sprite boyutuna göre ayarla
            polygonCollider.points = new Vector2[]
            {
                new Vector2(-width/2, -height/2),  // Sol alt
                new Vector2(width/2, -height/2),   // Sað alt
                new Vector2(0, height/2)           // Üst orta
            };
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Oyunu yeniden baþlat
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}