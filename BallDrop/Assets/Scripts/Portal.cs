using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (LevelCompleteManager.Instance != null)
                LevelCompleteManager.Instance.ShowLevelComplete();
        }
    }
}