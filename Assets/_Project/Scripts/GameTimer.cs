using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [Header("Timer Settings")]
    [SerializeField] private float startingTime = 120f;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    private float currentTime;
    private bool timerRunning = false;

    public bool IsTimeUp => currentTime <= 0f;
    public float CurrentTime => currentTime;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Jangan dihancurkan ketika pindah scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (!timerRunning)
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

    public void StartTimer()
    {
        currentTime = startingTime;
        timerRunning = true;

        UpdateTimerUI();
    }

    public void PauseTimer()
    {
        timerRunning = false;
    }

    public void ResumeTimer()
    {
        if (currentTime > 0f)
        {
            timerRunning = true;
        }
    }

    public void ResetTimer()
    {
        currentTime = startingTime;
        timerRunning = true;

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void TimeIsUp()
    {
        Debug.Log("GAME TIMER: TIME'S UP!");

        // Nanti kita sambungkan ke Game Over.
        // Untuk sekarang hanya menghentikan timer.
    }
}