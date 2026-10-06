using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Masukkan GameObject DimOverlay (yang menampung BriefingPanel)")]
    public GameObject briefingOverlay;

    [Header("Main Scene Elements")]
    [Tooltip("Masukkan GameObject karakter Pip utama yang ada di luar popup")]
    public GameObject mainPipCharacter; 

    void Start()
    {
        // Pastikan popup briefing tersembunyi saat pertama kali Main Menu dibuka
        if (briefingOverlay != null)
        {
            briefingOverlay.SetActive(false);
        }

        // Pastikan Pip utama kelihatan di awal
        if (mainPipCharacter != null)
        {
            mainPipCharacter.SetActive(true);
        }
    }

    // Dipanggil oleh tombol Start / Play
    public void OpenBriefing()
    {
        if (briefingOverlay != null)
        {
            briefingOverlay.SetActive(true);
        }

        // Sembunyikan Pip utama saat popup terbuka
        if (mainPipCharacter != null)
        {
            mainPipCharacter.SetActive(false);
        }
    }

    // Dipanggil oleh tombol [X] / Close
    public void CloseBriefing()
    {
        if (briefingOverlay != null)
        {
            briefingOverlay.SetActive(false);
        }

        // Munculkan kembali Pip utama saat popup ditutup
        if (mainPipCharacter != null)
        {
            mainPipCharacter.SetActive(true);
        }
    }

    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}