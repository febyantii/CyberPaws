using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level11Manager : MonoBehaviour
{
    [Header("PANELS")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject correctPanel;
    [SerializeField] private GameObject tryAgainPanel;

    [Header("DIALOGUE BUTTON")]
    [SerializeField] private Button continueButton;

    [Header("TRY AGAIN BUTTON")]
    [SerializeField] private Button tryAgainButton;

    [Header("GAME TEXTS")]
    [SerializeField] private TMP_Text sequenceText;
    [SerializeField] private TMP_Text inputText;
    [SerializeField] private TMP_Text timerText;

    [Header("ARROW BUTTONS")]
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [Header("GAME SETTINGS")]
    [SerializeField] private float timeLimit = 30f;

    [SerializeField] private KeyCode[] correctSequence =
    {
        KeyCode.UpArrow,
        KeyCode.RightArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.UpArrow,
        KeyCode.RightArrow
    };

    // Store player inputs
    private List<KeyCode> playerInput = new List<KeyCode>();

    // Timer
    private float timeRemaining;

    // Game state
    private bool gameIsActive = false;


    // ==========================================
    // INITIALIZATION
    // ==========================================

    private void Awake()
    {
        RegisterButtonEvents();
    }


    private void Start()
    {
        UpdateSequenceDisplay();

        ResetInput();

        timeRemaining = timeLimit;
        UpdateTimerDisplay();

        // Start with DialoguePanel
        ShowPanel(dialoguePanel);
    }


    // ==========================================
    // REGISTER BUTTON EVENTS
    // ==========================================

    private void RegisterButtonEvents()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(StartGame);
        }

        if (tryAgainButton != null)
        {
            tryAgainButton.onClick.AddListener(RestartGame);
        }

        if (upButton != null)
        {
            upButton.onClick.AddListener(
                () => HandlePlayerInput(KeyCode.UpArrow)
            );
        }

        if (downButton != null)
        {
            downButton.onClick.AddListener(
                () => HandlePlayerInput(KeyCode.DownArrow)
            );
        }

        if (leftButton != null)
        {
            leftButton.onClick.AddListener(
                () => HandlePlayerInput(KeyCode.LeftArrow)
            );
        }

        if (rightButton != null)
        {
            rightButton.onClick.AddListener(
                () => HandlePlayerInput(KeyCode.RightArrow)
            );
        }
    }


    // ==========================================
    // PANEL NAVIGATION
    // ==========================================

    private void ShowPanel(GameObject activePanel)
    {
        gameIsActive = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(activePanel == dialoguePanel);

        if (gamePanel != null)
            gamePanel.SetActive(activePanel == gamePanel);

        if (correctPanel != null)
            correctPanel.SetActive(activePanel == correctPanel);

        if (tryAgainPanel != null)
            tryAgainPanel.SetActive(activePanel == tryAgainPanel);
    }


    // ==========================================
    // START GAME
    // ==========================================

    private void StartGame()
    {
        if (correctSequence == null || correctSequence.Length == 0)
        {
            Debug.LogError("Correct sequence is empty!");
            return;
        }

        // Reset previous inputs
        ResetInput();

        // Reset timer
        timeRemaining = timeLimit;
        UpdateTimerDisplay();

        // Update sequence
        UpdateSequenceDisplay();

        // Show GamePanel and hide all other panels
        ShowPanel(gamePanel);

        // Activate gameplay
        gameIsActive = true;

        Debug.Log("Level 11 started!");
    }


    // ==========================================
    // KEYBOARD INPUT & TIMER
    // ==========================================

    private void Update()
    {
        if (!gameIsActive)
            return;

        // Countdown timer
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerDisplay();

            FailGame("Time is up!");
            return;
        }

        UpdateTimerDisplay();

        // Detect arrow keys only
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandlePlayerInput(KeyCode.UpArrow);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandlePlayerInput(KeyCode.DownArrow);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandlePlayerInput(KeyCode.LeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandlePlayerInput(KeyCode.RightArrow);
        }

        // Other keyboard keys are ignored
    }


    // ==========================================
    // PLAYER INPUT
    // ==========================================

    private void HandlePlayerInput(KeyCode pressedKey)
    {
        if (!gameIsActive)
            return;

        int currentIndex = playerInput.Count;

        // Prevent additional input after completion
        if (currentIndex >= correctSequence.Length)
            return;

        // Save player input
        playerInput.Add(pressedKey);

        // Update InputText
        UpdateInputDisplay();

        Debug.Log("Player pressed: " + pressedKey);

        // Check whether the input is correct
        if (pressedKey != correctSequence[currentIndex])
        {
            FailGame("Incorrect arrow sequence!");
            return;
        }

        // Check if all arrows have been entered
        if (playerInput.Count == correctSequence.Length)
        {
            WinGame();
        }
    }


    // ==========================================
    // SEQUENCE DISPLAY
    // ==========================================

    private void UpdateSequenceDisplay()
    {
        if (sequenceText == null)
            return;

        List<string> arrows = new List<string>();

        foreach (KeyCode key in correctSequence)
        {
            arrows.Add(GetArrowSymbol(key));
        }

        sequenceText.text = string.Join("  ", arrows);
    }


    // ==========================================
    // INPUT DISPLAY
    // ==========================================

    private void ResetInput()
    {
        playerInput.Clear();

        UpdateInputDisplay();
    }


    private void UpdateInputDisplay()
    {
        if (inputText == null)
            return;

        List<string> displayedInput = new List<string>();

        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (i < playerInput.Count)
            {
                displayedInput.Add(
                    GetArrowSymbol(playerInput[i])
                );
            }
            else
            {
                displayedInput.Add("•");
            }
        }

        inputText.text = string.Join("  ", displayedInput);
    }


    // ==========================================
    // ARROW SYMBOLS
    // ==========================================

    private string GetArrowSymbol(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow:
                return "↑";

            case KeyCode.DownArrow:
                return "↓";

            case KeyCode.LeftArrow:
                return "←";

            case KeyCode.RightArrow:
                return "→";

            default:
                return "?";
        }
    }


    // ==========================================
    // TIMER
    // ==========================================

    private void UpdateTimerDisplay()
    {
        if (timerText == null)
            return;

        int seconds = Mathf.CeilToInt(timeRemaining);

        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        timerText.text = string.Format(
            "{0:00}:{1:00}",
            minutes,
            remainingSeconds
        );
    }


    // ==========================================
    // GAME RESULT
    // ==========================================

    private void WinGame()
    {
        gameIsActive = false;

        Debug.Log("LEVEL 11 COMPLETED! +10 SECURITY POINTS");

        ShowPanel(correctPanel);
    }


    private void FailGame(string reason)
    {
        if (!gameIsActive)
            return;

        gameIsActive = false;

        Debug.Log("LEVEL 11 FAILED: " + reason);

        ShowPanel(tryAgainPanel);
    }


    // ==========================================
    // TRY AGAIN
    // ==========================================

    private void RestartGame()
    {
        Debug.Log("Restarting Level 11...");

        StartGame();
    }
}
