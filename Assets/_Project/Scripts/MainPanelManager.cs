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
        // Main Menu aktif
        mainMenuPanel.SetActive(true);

        // Semua Game Panel mati
        bellaGamePanel.SetActive(false);
        miaGamePanel.SetActive(false);
        frankieGamePanel.SetActive(false);

        // Bella = UNLOCKED
        bellaRequestCard.interactable = true;

        // Mia = LOCKED
        miaRequestCard.interactable = false;

        // Frankie = LOCKED
        frankieRequestCard.interactable = false;
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
        // Safety check
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
        // Safety check
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
        miaRequestCard.interactable = true;

        Debug.Log("Mia is now UNLOCKED!");
    }

    // =========================================================
    // UNLOCK FRANKIE
    // Dipanggil setelah Mia selesai
    // =========================================================

    public void UnlockFrankie()
    {
        frankieRequestCard.interactable = true;

        Debug.Log("Frankie is now UNLOCKED!");
    }

    // =========================================================
    // BACK TO MAIN MENU
    // =========================================================

    public void BackToMainMenu()
    {
        // Main Menu ON
        mainMenuPanel.SetActive(true);

        // Semua Game Panel OFF
        bellaGamePanel.SetActive(false);
        miaGamePanel.SetActive(false);
        frankieGamePanel.SetActive(false);

        Debug.Log("Back to Main Menu");
    }

    // =========================================================
    // LOCK BELLA
    // Dipakai setelah Bella selesai
    // =========================================================

    public void LockBella()
    {
        bellaRequestCard.interactable = false;

        Debug.Log("Bella is now LOCKED.");
    }

    // =========================================================
    // LOCK MIA
    // Dipakai setelah Mia selesai
    // =========================================================

    public void LockMia()
    {
        miaRequestCard.interactable = false;

        Debug.Log("Mia is now LOCKED.");
    }

    // =========================================================
    // LOCK FRANKIE
    // Dipakai setelah Frankie selesai
    // =========================================================

    public void LockFrankie()
    {
        frankieRequestCard.interactable = false;

        Debug.Log("Frankie is now LOCKED.");
    }
}

