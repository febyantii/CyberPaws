using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainScene : MonoBehaviour
{
    [Header("Scene Configuration")]
    public string mainAreaSceneName = "Act1_SuburbanDistrict";

    [Header("Save Configuration")]
    [Tooltip("Isi key PlayerPrefs sesuai minigame, misal: Minigame_Froggie")]
    public string minigameKey;

    public void CompleteMinigameAndReturn()
    {
        // Simpan status minigame selesai jika key diisi
        if (!string.IsNullOrEmpty(minigameKey))
        {
            PlayerPrefs.SetInt(minigameKey, 1);
            PlayerPrefs.Save();
        }

        // Pindah ke scene utama
        SceneManager.LoadScene(mainAreaSceneName);
    }
}