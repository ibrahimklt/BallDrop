using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteManager : MonoBehaviour
{
    public static LevelCompleteManager Instance { get; private set; }

    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Canvas canvas; // Canvas referansý ekledik

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        // Butonlara click olaylarýný ekle
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);

        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(LoadNextLevel);

        // Canvas'ý ayarla
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999; // En üstte görünsün
        }
    }

    public void ShowLevelComplete()
    {
        Time.timeScale = 0f;
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            levelCompletePanel.transform.SetAsLastSibling(); // En üstte görünsün
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        levelCompletePanel.SetActive(false);
        SceneManager.LoadScene("AnaMenu");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        levelCompletePanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        levelCompletePanel.SetActive(false);
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene("Level" + nextLevel);
    }
}