using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsPanel;  // Ayarlar paneli

    private void Start()
    {
        // Buton click olaylar�n� ekle
        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);

        // Ayarlar panelini ba�lang��ta gizle
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    private void StartGame()
    {
        // Level1 yerine LevelSelect sahnesine y�nlendir
        SceneManager.LoadScene("LevelSelect");
    }

    private void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
}