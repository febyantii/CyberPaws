using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ClosingManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Score Texts (Opsional)")]
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI loseScoreText;

    [Header("Scene Configuration")]
    public string mainMenuSceneName = "MainMenu"; // Ketik nama scene Main Menu kamu di sini

    private void Start()
    {
        // 1. Matikan kedua panel di awal agar tidak tumpang tindih
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        // 2. Langsung evaluasi skor saat scene dimuat
        ShowResult();
    }

    private void ShowResult()
    {
        // Mengambil skor terakhir dari memori sistem game kamu
        int finalScore = CityScoreManager.GetScore();

        if (finalScore >= 80)
        {
            winPanel.SetActive(true);
            if (winScoreText != null) winScoreText.text = $"{finalScore}%";
        }
        else
        {
            losePanel.SetActive(true);
            if (loseScoreText != null) loseScoreText.text = $"{finalScore}%";
        }
    }

    // Fungsi ini dipasang ke tombol "Back to Menu"
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}