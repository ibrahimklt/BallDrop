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
        // �nce mevcut butonlar� temizle
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Yeni butonlar� olu�tur
        for (int i = 1; i <= totalLevels; i++)
        {
            // Prefab'dan yeni buton olu�tur
            GameObject buttonObj = Instantiate(levelButtonPrefab, buttonContainer);
            buttonObj.name = "LevelButton_" + i; // Butonlar� isimlendir

            // Buton bile�enlerini al
            Button button = buttonObj.GetComponent<Button>();
            TMPro.TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            // Buton metnini ayarla
            buttonText.text = i.ToString();

            // �nceki level tamamland�ysa veya ilk level ise kilidi a�
            bool isUnlocked = PlayerPrefs.GetInt("Level" + (i - 1) + "Completed", 0) == 1 || i == 1;
            button.interactable = isUnlocked;

            // Click olay�n� ekle
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