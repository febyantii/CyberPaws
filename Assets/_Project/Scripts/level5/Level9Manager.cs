using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

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

    // Store the player's answers
    private PlayerAnswer[] playerAnswers;

    // Store the button images
    private Graphic[] legitGraphics;
    private Graphic[] scamGraphics;

    // Store original button colors
    private Color[] legitOriginalColors;
    private Color[] scamOriginalColors;


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

            // Get LEGIT button image
            legitGraphics[i] = PrepareButton(notifications[i].legitButton);

            // Get SCAM button image
            scamGraphics[i] = PrepareButton(notifications[i].scamButton);

            // Save original colors
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

        // Script controls the selection color
        button.transition = Selectable.Transition.None;

        Graphic graphic = button.targetGraphic;

        if (graphic == null)
            graphic = button.GetComponent<Graphic>();

        if (graphic == null)
        {
            Debug.LogWarning(
                "Button " + button.name + " does not have a Target Graphic."
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
            continueButton.onClick.AddListener(ShowNotificationPanel);
        }

        if (tryAgainButton != null)
        {
            tryAgainButton.onClick.AddListener(RestartNotifications);
        }

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
    }


    private void ShowNotificationPanel()
    {
        SetActivePanel(notifPanel);
    }


    private void ShowResultPanel(bool allCorrect)
    {
        if (allCorrect)
        {
            SetActivePanel(correctPanel);
            Debug.Log("LEVEL 9 COMPLETED! All answers are correct.");
        }
        else
        {
            SetActivePanel(tryAgainPanel);
            Debug.Log("Some answers are incorrect. Try again!");
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
    // PLAYER SELECTS LEGIT / SCAM
    // ==========================================

    private void SelectAnswer(int index, PlayerAnswer selectedAnswer)
    {
        if (notifPanel == null || !notifPanel.activeInHierarchy)
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
            // Select or switch to the other answer
            playerAnswers[index] = selectedAnswer;
        }

        // Update selected button colors
        UpdateButtonColors(index);

        Debug.Log(
            "Notification " + (index + 1) +
            " selected: " + playerAnswers[index]
        );

        // Check whether all notifications have been answered
        CheckAllAnswers();
    }


    // ==========================================
    // BUTTON COLORS
    // ==========================================

    private void UpdateButtonColors(int index)
    {
        if (playerAnswers[index] == PlayerAnswer.Legit)
        {
            // LEGIT selected
            if (legitGraphics[index] != null)
                legitGraphics[index].color = selectedLegitColor;

            // SCAM unselected
            if (scamGraphics[index] != null)
                scamGraphics[index].color = scamOriginalColors[index];
        }
        else if (playerAnswers[index] == PlayerAnswer.Scam)
        {
            // SCAM selected
            if (scamGraphics[index] != null)
                scamGraphics[index].color = selectedScamColor;

            // LEGIT unselected
            if (legitGraphics[index] != null)
                legitGraphics[index].color = legitOriginalColors[index];
        }
        else
        {
            // Both buttons return to original colors
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
        // Wait until all notifications have been answered
        for (int i = 0; i < playerAnswers.Length; i++)
        {
            if (playerAnswers[i] == PlayerAnswer.None)
                return;
        }

        bool allCorrect = true;

        // Compare the player's answers with correct answers
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

        // Automatically show the result
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
        // Clear all previous answers
        ResetAllAnswers();

        // Return to notification page
        ShowNotificationPanel();

        Debug.Log("Notifications have been reset.");
    }
}
