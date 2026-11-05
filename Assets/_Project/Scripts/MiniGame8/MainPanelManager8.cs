
using UnityEngine;
using UnityEngine.UI;

public class MainPanelManager8 : MonoBehaviour
{
    [Header("MAIN GAME PANEL")]
    public GameObject mainGamePanel;

    [Header("CHARACTER PANELS")]
    public GameObject paulaGamePanel;
    public GameObject kevinGamePanel;
    public GameObject bellaGamePanel;

    [Header("CHARACTER BUTTONS")]
    public Button paulaButton;
    public Button kevinButton;
    public Button bellaButton;

    [Header("TIMER MANAGER")]
    public GameTimerManager timerManager;

    [Header("PAULA MANAGER")]
    public PaulaGameManager paulaManager;

    private bool paulaCompleted = false;
    private bool kevinCompleted = false;
    private bool bellaCompleted = false;

    private void Start()
    {
        paulaButton.onClick.AddListener(StartPaula);
        kevinButton.onClick.AddListener(StartKevin);
        bellaButton.onClick.AddListener(StartBella);

        ShowMainGamePanel();
    }

    // =====================================
    // MAIN PANEL
    // =====================================

    public void ShowMainGamePanel()
    {
        mainGamePanel.SetActive(true);

        paulaGamePanel.SetActive(false);
        kevinGamePanel.SetActive(false);
        bellaGamePanel.SetActive(false);

        UpdateCharacterButtons();
    }

    // =====================================
    // START CHARACTER
    // =====================================

    public void StartPaula()
    {
        if (timerManager.IsGameFailed || paulaCompleted)
            return;

        mainGamePanel.SetActive(false);
        paulaGamePanel.SetActive(true);
    }

    public void StartKevin()
    {
        if (timerManager.IsGameFailed ||
            !paulaCompleted || kevinCompleted)
            return;

        mainGamePanel.SetActive(false);
        kevinGamePanel.SetActive(true);
    }

    public void StartBella()
    {
        if (timerManager.IsGameFailed ||
            !kevinCompleted || bellaCompleted)
            return;

        mainGamePanel.SetActive(false);
        bellaGamePanel.SetActive(true);
    }

    // =====================================
    // CHARACTER COMPLETION
    // =====================================

    public void MarkPaulaCompleted()
    {
        if (timerManager.IsGameFailed)
            return;

        paulaCompleted = true;

        ShowMainGamePanel();
    }

    public void MarkKevinCompleted()
    {
        if (timerManager.IsGameFailed || !paulaCompleted)
            return;

        kevinCompleted = true;

        ShowMainGamePanel();
    }

    public void MarkBellaCompleted()
    {
        if (timerManager.IsGameFailed || !kevinCompleted)
            return;

        bellaCompleted = true;

        // Stop timer when all levels are completed
        timerManager.StopTimer();

        ShowMainGamePanel();
    }

    // =====================================
    // CHARACTER BUTTON INTERACTION
    // =====================================

    private void UpdateCharacterButtons()
    {
        paulaButton.interactable =
            !paulaCompleted && !timerManager.IsGameFailed;

        kevinButton.interactable =
            paulaCompleted &&
            !kevinCompleted &&
            !timerManager.IsGameFailed;

        bellaButton.interactable =
            kevinCompleted &&
            !bellaCompleted &&
            !timerManager.IsGameFailed;
    }

    // =====================================
    // RESET GAME PROGRESS
    // =====================================

    public void ResetProgressForRestart()
    {
        paulaCompleted = false;
        kevinCompleted = false;
        bellaCompleted = false;

        if (paulaManager != null)
        {
            paulaManager.ResetPaulaProgress();
        }

        ShowMainGamePanel();
    }
}
