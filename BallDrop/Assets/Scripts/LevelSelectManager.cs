using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private int totalLevels = 11;
    [SerializeField] private Button backButton;

    private void Start()
    {
        // Önce mevcut butonlarý temizle
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Yeni butonlarý oluþtur
        for (int i = 1; i <= totalLevels; i++)
        {
            // Prefab'dan yeni buton oluþtur
            GameObject buttonObj = Instantiate(levelButtonPrefab, buttonContainer);
            buttonObj.name = "LevelButton_" + i; // Butonlarý isimlendir

            // Buton bileþenlerini al
            Button button = buttonObj.GetComponent<Button>();
            TMPro.TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            // Buton metnini ayarla
            buttonText.text = i.ToString();

            // Önceki level tamamlandýysa veya ilk level ise kilidi aç
            bool isUnlocked = PlayerPrefs.GetInt("Level" + (i - 1) + "Completed", 0) == 1 || i == 1;
            button.interactable = isUnlocked;

            // Click olayýný ekle
            int levelIndex = i;
            button.onClick.AddListener(() => LoadLevel(levelIndex));
        }

        SetupBackButton();
    }

    private void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }

    private void SetupBackButton()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(() => SceneManager.LoadScene("AnaMenu"));
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("AnaMenu");
    }
}