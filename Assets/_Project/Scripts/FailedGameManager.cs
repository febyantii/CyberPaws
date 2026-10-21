
using UnityEngine;

public class FailedGameManager : MonoBehaviour
{
    [Header("========== FAILED PANEL ==========")]
    [SerializeField] private GameObject failedPanel;

    [Header("========== MAIN PANEL MANAGER ==========")]
    [SerializeField] private MainPanelManager mainPanelManager;

    [Header("========== GAME TIMER ==========")]
    [SerializeField] private GameTimer gameTimer;

    [Header("========== NPC GAME MANAGERS ==========")]
    [SerializeField] private BellaGameManager bellaGameManager;
    [SerializeField] private MiaGameManager miaGameManager;
    [SerializeField] private FrankieGameManager frankieGameManager;

    [Header("========== FRANKIE GAME DETAIL ==========")]
    [SerializeField] private FrankieGameDetailManager frankieGameDetailManager;


    // =====================================================
    // RESTART ENTIRE MINIGAME 3
    // =====================================================

    public void RestartGame()
    {
        Debug.Log("Restarting MiniGame3 from the beginning...");

        // 1. Cari timer jika belum dihubungkan
        if (gameTimer == null)
            gameTimer = GameTimer.Instance;

        // 2. Hentikan timer selama proses reset
        if (gameTimer != null)
            gameTimer.PauseTimer();

        // 3. Reset semua panel dan status NPC
        if (mainPanelManager != null)
        {
            mainPanelManager.ResetToInitialState();
        }
        else
        {
            Debug.LogWarning(
                "FailedGameManager: MainPanelManager belum dihubungkan!"
            );
        }

        // 4. Reset seluruh progres Bella
        if (bellaGameManager != null)
        {
            bellaGameManager.ResetBellaGame();
        }
        else
        {
            Debug.LogWarning(
                "FailedGameManager: BellaGameManager belum dihubungkan!"
            );
        }

        // 5. Reset seluruh progres Mia dan kartu
        if (miaGameManager != null)
        {
            miaGameManager.ResetMiaGame();
        }
        else
        {
            Debug.LogWarning(
                "FailedGameManager: MiaGameManager belum dihubungkan!"
            );
        }

        // 6. Reset Frankie ke halaman profil
        if (frankieGameManager != null)
        {
            frankieGameManager.ResetFrankieGame();
        }
        else
        {
            Debug.LogWarning(
                "FailedGameManager: FrankieGameManager belum dihubungkan!"
            );
        }

        // 7. Reset pola dan kesempatan melihat pola Frankie
        if (frankieGameDetailManager != null)
        {
            frankieGameDetailManager.ResetFrankieGameDetail();
        }
        else
        {
            Debug.LogWarning(
                "FailedGameManager: FrankieGameDetailManager belum dihubungkan!"
            );
        }

        // 8. Tutup Failed Panel
        if (failedPanel != null)
        {
            failedPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "FailedGameManager: FailedPanel belum dihubungkan!"
            );
        }

        // 9. Reset timer global dan mulai kembali dari 02:00
        if (gameTimer != null)
        {
            gameTimer.ResetTimer();
        }
        else
        {
            Debug.LogWarning(
                "FailedGameManager: GameTimer tidak ditemukan!"
            );
        }

        Debug.Log("MiniGame3 restarted successfully.");
    }
}
