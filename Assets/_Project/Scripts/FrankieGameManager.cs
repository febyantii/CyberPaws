using UnityEngine;

public class FrankieGameManager : MonoBehaviour
{
    [Header("========== MAIN PANELS ==========")]
    [SerializeField] private GameObject characterProfilePanel;
    [SerializeField] private GameObject smsPanel;
    [SerializeField] private GameObject gameDetailPanel;

    [Header("========== RESULT PANELS ==========")]
    [SerializeField] private GameObject congratulationPanel;
    [SerializeField] private GameObject tryAgainPanel;

    [Header("========== MAIN PANEL MANAGER ==========")]
    [SerializeField] private MainPanelManager mainPanelManager;


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        SetupFrankie();
    }


    // =====================================================
    // INITIAL SETUP
    // =====================================================

    private void SetupFrankie()
    {
        if (characterProfilePanel != null)
            characterProfilePanel.SetActive(true);

        if (smsPanel != null)
            smsPanel.SetActive(false);

        if (gameDetailPanel != null)
            gameDetailPanel.SetActive(false);

        if (congratulationPanel != null)
            congratulationPanel.SetActive(false);

        if (tryAgainPanel != null)
            tryAgainPanel.SetActive(false);
    }


    // =====================================================
    // PROFILE → SMS
    // =====================================================

    public void ContinueFromProfile()
    {
        if (characterProfilePanel != null)
            characterProfilePanel.SetActive(false);

        if (smsPanel != null)
            smsPanel.SetActive(true);

        Debug.Log("FRANKIE → SMS Panel");
    }


    // =====================================================
    // SMS → GAME DETAIL
    // =====================================================

    public void CheckDevice()
    {
        if (smsPanel != null)
            smsPanel.SetActive(false);

        if (gameDetailPanel != null)
            gameDetailPanel.SetActive(true);

        Debug.Log("FRANKIE → Game Detail");
    }


    // =====================================================
    // GAME DETAIL → TRY AGAIN
    // =====================================================

    public void ShowTryAgain()
    {
        if (gameDetailPanel != null)
            gameDetailPanel.SetActive(false);

        if (tryAgainPanel != null)
            tryAgainPanel.SetActive(true);

        Debug.Log("FRANKIE → Try Again Panel");
    }


    // =====================================================
    // TRY AGAIN → GAME DETAIL
    // =====================================================

    public void TryAgain()
    {
        if (tryAgainPanel != null)
            tryAgainPanel.SetActive(false);

        if (gameDetailPanel != null)
            gameDetailPanel.SetActive(true);

        Debug.Log("FRANKIE → Game Detail Again");
    }


    // =====================================================
    // GAME DETAIL → CONGRATULATION
    // =====================================================

    public void ShowCongratulation()
    {
        if (gameDetailPanel != null)
            gameDetailPanel.SetActive(false);

        if (congratulationPanel != null)
            congratulationPanel.SetActive(true);

        Debug.Log("FRANKIE → CONGRATULATION!");
    }


    // =====================================================
    // CONGRATULATION → MAIN MENU
    // =====================================================

    public void BackToHome()
    {
        if (congratulationPanel != null)
            congratulationPanel.SetActive(false);

        if (mainPanelManager != null)
        {
            // Frankie selesai
            mainPanelManager.LockFrankie();

            // Kembali ke Main Menu
            mainPanelManager.BackToMainMenu();
        }

        Debug.Log("FRANKIE COMPLETED → Back to Main Menu");
    }
}