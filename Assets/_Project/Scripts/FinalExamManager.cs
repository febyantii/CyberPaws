using System.Collections; // Wajib ditambahin buat pakai Coroutine
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalExamManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject finalExamPopupPanel;
    public Button nextButton;

    [Header("Scene Configuration")]
    public string finalExamSceneName = "MiniGame3"; 

    [Header("Delay Settings")]
    [Tooltip("Berapa detik jeda sebelum popup muncul setelah balik ke scene ini?")]
    public float popupDelay = 1.5f;

    private void Start()
    {
        // 1. PASTIKAN POPUP MATI DULU DI AWAL
        if (finalExamPopupPanel != null)
        {
            finalExamPopupPanel.SetActive(false);
        }

        // 2. Jalankan pengecekan tapi pakai delay
        StartCoroutine(CheckProgressWithDelay());
    }

    // Coroutine untuk memberi jeda sebelum ngecek dan nampilin popup
    private IEnumerator CheckProgressWithDelay()
    {
        yield return new WaitForSeconds(popupDelay);

        // Tarik data mentahnya dulu
        int froggie = PlayerPrefs.GetInt("Minigame_Froggie", 0);
        int benny = PlayerPrefs.GetInt("Minigame_Benny", 0);
        int macy = PlayerPrefs.GetInt("Minigame_Macy", 0);
        int popup = PlayerPrefs.GetInt("FinalExamPopupShown", 0);

        // Cetak ke Console biar kelihatan jelas!
        Debug.Log($"[CEK DATA] Froggie: {froggie} | Benny: {benny} | Macy: {macy} | PopupShown: {popup}");

        // Cek syaratnya
        if (froggie == 1 && benny == 1 && macy == 1 && popup == 0)
        {
            Debug.Log("[CEK DATA] Semua syarat terpenuhi! Memunculkan popup...");
            ShowPopup();
        }
        else
        {
            Debug.Log("[CEK DATA] Popup ditahan karena ada data yang belum bernilai 1 atau popup sudah pernah muncul.");
        }
    }

    private void ShowPopup()
    {
        finalExamPopupPanel.SetActive(true);

        // Bekukan waktu game biar player gak bisa jalan-jalan
        Time.timeScale = 0f; 

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
    }

    private void OnNextButtonClicked()
    {
        // Balikin waktu ke normal sebelum pindah scene
        Time.timeScale = 1f; 

        // Tandai biar gak muncul lagi kalau balik ke kota
        PlayerPrefs.SetInt("FinalExamPopupShown", 1);
        PlayerPrefs.Save();

        // Gass ke Final Exam
        SceneManager.LoadScene(finalExamSceneName);
    }

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