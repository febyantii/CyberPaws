
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager Instance;

    [Header("TIMER SETTINGS")]
    [SerializeField] private float startingTime = 120f;

    [Header("TIMER UI")]
    [SerializeField] private TMP_Text timerText;

    [Header("GAME PANELS")]
    [SerializeField] private GameObject mainGamePanel;
    [SerializeField] private GameObject paulaGamePanel;
    [SerializeField] private GameObject kevinGamePanel;
    [SerializeField] private GameObject bellaGamePanel;

    [Header("FAILED GAME PANEL")]
    [SerializeField] private GameObject failedGamePanel;
    [SerializeField] private Button restartButton;

    [Header("MAIN PANEL MANAGER")]
    [SerializeField] private MainPanelManager8 mainPanelManager;

    private float currentTime;
    private bool timerRunning = false;
    private bool gameFailed = false;

    public bool IsTimeUp => currentTime <= 0f;
    public bool IsGameFailed => gameFailed;
    public float CurrentTime => currentTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Debug.Log("GAME TIMER MANAGER STARTED!");

        if (failedGamePanel != null)
        {
            failedGamePanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogError("Restart Button belum dihubungkan!");
        }

        StartTimer();

        Debug.Log("Starting Time: " + startingTime);
        Debug.Log("Current Time: " + currentTime);
        Debug.Log("Timer Running: " + timerRunning);
    }

    private void Update()
    {
        if (!timerRunning || gameFailed)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            timerRunning = false;

            UpdateTimerUI();
            TimeIsUp();

            return;
        }

        UpdateTimerUI();
    }

    // =====================================
    // START TIMER
    // =====================================

    public void StartTimer()
    {
        currentTime = startingTime;
        timerRunning = true;
        gameFailed = false;

        UpdateTimerUI();
    }

    // =====================================
    // PAUSE AND RESUME
    // =====================================

    public void PauseTimer()
    {
        timerRunning = false;
    }

    public void ResumeTimer()
    {
        if (currentTime > 0f && !gameFailed)
        {
            timerRunning = true;
        }
    }

    // =====================================
    // STOP TIMER
    // =====================================

    public void StopTimer()
    {
        timerRunning = false;
    }

    // =====================================
    // RESET TIMER
    // =====================================

    public void ResetTimer()
    {
        currentTime = startingTime;
        timerRunning = true;
        gameFailed = false;

        if (failedGamePanel != null)
        {
            failedGamePanel.SetActive(false);
        }

        UpdateTimerUI();
    }

    // =====================================
    // UPDATE TIMER TEXT
    // =====================================

    private void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int totalSeconds = Mathf.CeilToInt(currentTime);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // =====================================
    // TIME IS UP
    // =====================================

    private void TimeIsUp()
    {
        Debug.Log("MINIGAME 8: TIME'S UP!");

        gameFailed = true;

        mainGamePanel.SetActive(false);
        paulaGamePanel.SetActive(false);
        kevinGamePanel.SetActive(false);
        bellaGamePanel.SetActive(false);

        if (failedGamePanel != null)
        {
            failedGamePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "GameTimerManager: FailedGamePanel belum dihubungkan!"
            );
        }
    }

    // =====================================
    // RESTART GAME
    // =====================================

    public void RestartGame()
    {
        ResetTimer();

        if (mainPanelManager != null)
        {
            mainPanelManager.ResetProgressForRestart();
        }
        else
        {
            Debug.LogWarning(
                "GameTimerManager: MainPanelManager8 belum dihubungkan!"
            );
        }

        Debug.Log("MINIGAME 8: GAME RESTARTED!");
    }
}
