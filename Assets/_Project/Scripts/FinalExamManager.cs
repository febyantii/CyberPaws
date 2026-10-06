using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalExamManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject finalExamPopupPanel;
    public Button nextButton;

    [Header("Scene Configuration")]
    public string finalExamSceneName = "FinalExamScene"; // Nama scene minigame terakhir

    private void Start()
    {
        // Pastikan popup mati di awal
        if (finalExamPopupPanel != null)
        {
            finalExamPopupPanel.SetActive(false);
        }

        CheckProgressAndShowPopup();
    }

    private void CheckProgressAndShowPopup()
    {
        // 1. Cek status penyelesaian 3 minigame (1 = Selesai, 0 = Belum)
        bool isFroggieDone = PlayerPrefs.GetInt("Minigame_Froggie", 0) == 1;
        bool isBennyDone = PlayerPrefs.GetInt("Minigame_Benny", 0) == 1;
        bool isMacyDone = PlayerPrefs.GetInt("Minigame_Macy", 0) == 1;

        // 2. Cek apakah popup ujian akhir sudah pernah ditampilkan sebelumnya
        bool isPopupAlreadyShown = PlayerPrefs.GetInt("FinalExamPopupShown", 0) == 1;

        // 3. Jika semua minigame kelar & popup belum pernah muncul
        if (isFroggieDone && isBennyDone && isMacyDone && !isPopupAlreadyShown)
        {
            ShowPopup();
        }
    }

    private void ShowPopup()
    {
        finalExamPopupPanel.SetActive(true);

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
    }

    private void OnNextButtonClicked()
    {
        // Tandai bahwa popup sudah dimunculkan agar tidak berulang saat kembali lagi
        PlayerPrefs.SetInt("FinalExamPopupShown", 1);
        PlayerPrefs.Save();

        // Pindah ke scene minigame ujian akhir
        SceneManager.LoadScene(finalExamSceneName);
    }

    // [Opsional] Panggil fungsi ini jika mau reset progress saat testing
    [ContextMenu("Reset All Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("Minigame_Froggie");
        PlayerPrefs.DeleteKey("Minigame_Benny");
        PlayerPrefs.DeleteKey("Minigame_Macy");
        PlayerPrefs.DeleteKey("FinalExamPopupShown");
        PlayerPrefs.Save();
        Debug.Log("Progress Minigame Berhasil Direset!");
    }
}