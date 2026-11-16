using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level9Manager : MonoBehaviour
{
    public enum PlayerAnswer
    {
        None,
        Legit,
        Scam
    }

    [Serializable]
    public class NotificationEntry
    {
        public Button legitButton;
        public Button scamButton;
        public PlayerAnswer correctAnswer;
    }

    [Header("PANELS")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject notifPanel;
    [SerializeField] private GameObject correctPanel;
    [SerializeField] private GameObject tryAgainPanel;

    [Header("DIALOGUE BUTTON")]
    [SerializeField] private Button continueButton;

    [Header("TRY AGAIN BUTTON")]
    [SerializeField] private Button tryAgainButton;

    [Header("NOTIFICATION BUTTONS")]
    [SerializeField] private NotificationEntry[] notifications =
        new NotificationEntry[3];

    [Header("SELECTED BUTTON COLORS")]
    [SerializeField] private Color selectedLegitColor =
        new Color(0.60f, 1f, 0.65f, 1f);

    [SerializeField] private Color selectedScamColor =
        new Color(1f, 0.60f, 0.60f, 1f);

    [Header("TIMER")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeLimit = 60f;

    // Player answers
    private PlayerAnswer[] playerAnswers;

    // Button images
    private Graphic[] legitGraphics;
    private Graphic[] scamGraphics;

    // Original button colors
    private Color[] legitOriginalColors;
    private Color[] scamOriginalColors;

    // Timer variables
    private float timeRemaining;
    private Coroutine timerCoroutine;


    private void Awake()
    {
        InitializeButtons();
        RegisterButtonEvents();
    }


    private void Start()
    {
        ResetAllAnswers();
        ShowDialoguePanel();
    }


    // ==========================================
    // INITIALIZE BUTTONS
    // ==========================================

    private void InitializeButtons()
    {
        int count = notifications != null ? notifications.Length : 0;

        playerAnswers = new PlayerAnswer[count];

        legitGraphics = new Graphic[count];
        scamGraphics = new Graphic[count];

        legitOriginalColors = new Color[count];
        scamOriginalColors = new Color[count];

        for (int i = 0; i < count; i++)
        {
            if (notifications[i] == null)
                continue;

            legitGraphics[i] =
                PrepareButton(notifications[i].legitButton);

            scamGraphics[i] =
                PrepareButton(notifications[i].scamButton);

            if (legitGraphics[i] != null)
                legitOriginalColors[i] = legitGraphics[i].color;

            if (scamGraphics[i] != null)
                scamOriginalColors[i] = scamGraphics[i].color;
        }
    }


    private Graphic PrepareButton(Button button)
    {
        if (button == null)
            return null;

        button.transition = Selectable.Transition.None;

        Graphic graphic = button.targetGraphic;

        if (graphic == null)
            graphic = button.GetComponent<Graphic>();

        if (graphic == null)
        {
            Debug.LogWarning(
                "Button " + button.name +
                " does not have a Target Graphic."
            );
        }

        return graphic;
    }


    // ==========================================
    // REGISTER BUTTON EVENTS
    // ==========================================

    private void RegisterButtonEvents()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(
                ShowNotificationPanel
            );
        }

        if (tryAgainButton != null)
        {
            tryAgainButton.onClick.AddListener(
                RestartNotifications
            );
        }

        if (notifications == null)
            return;

        for (int i = 0; i < notifications.Length; i++)
        {
            if (notifications[i] == null)
                continue;

            int index = i;

            if (notifications[i].legitButton != null)
            {
                notifications[i].legitButton.onClick.AddListener(
                    () => SelectAnswer(index, PlayerAnswer.Legit)
                );
            }

            if (notifications[i].scamButton != null)
            {
                notifications[i].scamButton.onClick.AddListener(
                    () => SelectAnswer(index, PlayerAnswer.Scam)
                );
            }
        }
    }


    // ==========================================
    // PANEL NAVIGATION
    // ==========================================

    private void ShowDialoguePanel()
    {
        SetActivePanel(dialoguePanel);
        StopTimer();
    }


    private void ShowNotificationPanel()
    {
        SetActivePanel(notifPanel);

        // Start a fresh 60-second timer
        StartTimer();
    }


    private void ShowResultPanel(bool allCorrect)
    {
        // Stop timer when result appears
        StopTimer();

        if (allCorrect)
        {
            SetActivePanel(correctPanel);

            Debug.Log(
                "LEVEL 9 COMPLETED! All answers are correct."
            );
        }
        else
        {
            SetActivePanel(tryAgainPanel);

            Debug.Log(
                "Some answers are incorrect or time is up."
            );
        }
    }


    private void SetActivePanel(GameObject activePanel)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(activePanel == dialoguePanel);

        if (notifPanel != null)
            notifPanel.SetActive(activePanel == notifPanel);

        if (correctPanel != null)
            correctPanel.SetActive(activePanel == correctPanel);

        if (tryAgainPanel != null)
            tryAgainPanel.SetActive(activePanel == tryAgainPanel);
    }


    // ==========================================
    // TIMER SYSTEM
    // ==========================================

    private void StartTimer()
    {
        // Stop any previous timer
        StopTimer();

        // Reset timer to 60 seconds
        timeRemaining = timeLimit;

        UpdateTimerDisplay();

        // Start countdown
        timerCoroutine = StartCoroutine(CountdownTimer());

        Debug.Log("Timer started: " + timeLimit + " seconds.");
    }


    private IEnumerator CountdownTimer()
    {
        while (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining < 0f)
                timeRemaining = 0f;

            UpdateTimerDisplay();

            if (timeRemaining <= 0f)
                break;

            yield return null;
        }

        timerCoroutine = null;

        // Time is up only if notification page is still active
        if (notifPanel != null && notifPanel.activeInHierarchy)
        {
            Debug.Log("TIME'S UP!");

            ShowResultPanel(false);
        }
    }


    private void UpdateTimerDisplay()
    {
        if (timerText == null)
            return;

        int totalSeconds = Mathf.CeilToInt(timeRemaining);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = string.Format(
            "{0:00}:{1:00}",
            minutes,
            seconds
        );
    }


    private void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }


    // ==========================================
    // PLAYER SELECTS LEGIT / SCAM
    // ==========================================

    private void SelectAnswer(
        int index,
        PlayerAnswer selectedAnswer
    )
    {
        if (notifPanel == null ||
            !notifPanel.activeInHierarchy)
            return;

        if (index < 0 || index >= playerAnswers.Length)
            return;

        // Click the same button again to unselect
        if (playerAnswers[index] == selectedAnswer)
        {
            playerAnswers[index] = PlayerAnswer.None;
        }
        else
        {
            // Select or switch answer
            playerAnswers[index] = selectedAnswer;
        }

        UpdateButtonColors(index);

        Debug.Log(
            "Notification " + (index + 1) +
            " selected: " + playerAnswers[index]
        );

        CheckAllAnswers();
    }


    // ==========================================
    // BUTTON COLORS
    // ==========================================

    private void UpdateButtonColors(int index)
    {
        if (playerAnswers[index] == PlayerAnswer.Legit)
        {
            if (legitGraphics[index] != null)
                legitGraphics[index].color = selectedLegitColor;

            if (scamGraphics[index] != null)
                scamGraphics[index].color = scamOriginalColors[index];
        }
        else if (playerAnswers[index] == PlayerAnswer.Scam)
        {
            if (scamGraphics[index] != null)
                scamGraphics[index].color = selectedScamColor;

            if (legitGraphics[index] != null)
                legitGraphics[index].color = legitOriginalColors[index];
        }
        else
        {
            if (legitGraphics[index] != null)
                legitGraphics[index].color = legitOriginalColors[index];

            if (scamGraphics[index] != null)
                scamGraphics[index].color = scamOriginalColors[index];
        }
    }


    // ==========================================
    // CHECK ALL ANSWERS
    // ==========================================

    private void CheckAllAnswers()
    {
        // Wait until all notifications are answered
        for (int i = 0; i < playerAnswers.Length; i++)
        {
            if (playerAnswers[i] == PlayerAnswer.None)
                return;
        }

        bool allCorrect = true;

        // Compare player answers with correct answers
        for (int i = 0; i < playerAnswers.Length; i++)
        {
            if (notifications[i] == null ||
                notifications[i].correctAnswer == PlayerAnswer.None ||
                playerAnswers[i] != notifications[i].correctAnswer)
            {
                allCorrect = false;
                break;
            }
        }

        ShowResultPanel(allCorrect);
    }


    // ==========================================
    // RESET ANSWERS / TRY AGAIN
    // ==========================================

    private void ResetAllAnswers()
    {
        for (int i = 0; i < playerAnswers.Length; i++)
        {
            playerAnswers[i] = PlayerAnswer.None;

            UpdateButtonColors(i);
        }
    }


    private void RestartNotifications()
    {
        // Reset all selected answers
        ResetAllAnswers();

        // Return to notification page
        // ShowNotificationPanel automatically restarts timer
        ShowNotificationPanel();

        Debug.Log(
            "Notifications reset. Timer restarted."
        );
    }
}
