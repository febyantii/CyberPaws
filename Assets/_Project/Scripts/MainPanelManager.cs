
using UnityEngine;
using UnityEngine.UI;

public class MainPanelManager : MonoBehaviour
{
    [Header("========== REQUEST CARDS ==========")]
    [SerializeField] private Button bellaRequestCard;
    [SerializeField] private Button miaRequestCard;
    [SerializeField] private Button frankieRequestCard;

    [Header("========== GAME PANELS ==========")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject bellaGamePanel;
    [SerializeField] private GameObject miaGamePanel;
    [SerializeField] private GameObject frankieGamePanel;

    private void Start()
    {
        SetupInitialState();
    }

    // =========================================================
    // INITIAL STATE
    // =========================================================

    private void SetupInitialState()
    {
        ResetToInitialState();
    }

    // =========================================================
    // RESET ENTIRE MINI GAME 3 TO INITIAL STATE
    // Dipanggil ketika player menekan tombol Restart
    // =========================================================

    public void ResetToInitialState()
    {
        // Main Menu aktif
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        // Semua Game Panel mati
        if (bellaGamePanel != null)
            bellaGamePanel.SetActive(false);

        if (miaGamePanel != null)
            miaGamePanel.SetActive(false);

        if (frankieGamePanel != null)
            frankieGamePanel.SetActive(false);

        // Bella = UNLOCKED
        if (bellaRequestCard != null)
            bellaRequestCard.interactable = true;

        // Mia = LOCKED
        if (miaRequestCard != null)
            miaRequestCard.interactable = false;

        // Frankie = LOCKED
        if (frankieRequestCard != null)
            frankieRequestCard.interactable = false;

        Debug.Log("MiniGame3 reset to initial state.");
    }

    // =========================================================
    // OPEN BELLA
    // =========================================================

    public void OpenBella()
    {
        mainMenuPanel.SetActive(false);
        bellaGamePanel.SetActive(true);

        Debug.Log("Opening Bella Game");
    }

    // =========================================================
    // OPEN MIA
    // =========================================================

    public void OpenMia()
    {
        if (!miaRequestCard.interactable)
        {
            Debug.Log("Mia is still locked!");
            return;
        }

        mainMenuPanel.SetActive(false);
        miaGamePanel.SetActive(true);

        Debug.Log("Opening Mia Game");
    }

    // =========================================================
    // OPEN FRANKIE
    // =========================================================

    public void OpenFrankie()
    {
        if (!frankieRequestCard.interactable)
        {
            Debug.Log("Frankie is still locked!");
            return;
        }

        mainMenuPanel.SetActive(false);
        frankieGamePanel.SetActive(true);

        Debug.Log("Opening Frankie Game");
    }

    // =========================================================
    // UNLOCK MIA
    // Dipanggil setelah Bella selesai
    // =========================================================

    public void UnlockMia()
    {
        if (miaRequestCard != null)
            miaRequestCard.interactable = true;

        Debug.Log("Mia is now UNLOCKED!");
    }

    // =========================================================
    // UNLOCK FRANKIE
    // Dipanggil setelah Mia selesai
    // =========================================================

    public void UnlockFrankie()
    {
        if (frankieRequestCard != null)
            frankieRequestCard.interactable = true;

        Debug.Log("Frankie is now UNLOCKED!");
    }

    // =========================================================
    // BACK TO MAIN MENU
    // =========================================================

    public void BackToMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        if (bellaGamePanel != null)
            bellaGamePanel.SetActive(false);

        if (miaGamePanel != null)
            miaGamePanel.SetActive(false);

        if (frankieGamePanel != null)
            frankieGamePanel.SetActive(false);

        Debug.Log("Back to Main Menu");
    }

    // =========================================================
    // LOCK BELLA
    // =========================================================

    public void LockBella()
    {
        if (bellaRequestCard != null)
            bellaRequestCard.interactable = false;

        Debug.Log("Bella is now LOCKED.");
    }

    // =========================================================
    // LOCK MIA
    // =========================================================

    public void LockMia()
    {
        if (miaRequestCard != null)
            miaRequestCard.interactable = false;

        Debug.Log("Mia is now LOCKED.");
    }

    // =========================================================
    // LOCK FRANKIE
    // =========================================================

    public void LockFrankie()
    {
        if (frankieRequestCard != null)
            frankieRequestCard.interactable = false;

        Debug.Log("Frankie is now LOCKED.");
    }
}
