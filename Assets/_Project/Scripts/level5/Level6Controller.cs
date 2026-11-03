using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level6Controller : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject findTheDifferencePanel;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject tryAgainPanel;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float roundDuration = 30f;

    [Header("Found Indicators")]
    [SerializeField] private GameObject logoFound;
    [SerializeField] private GameObject developerFound;
    [SerializeField] private GameObject permissionFound;

    private float timeRemaining;
    private bool isTimerRunning;

    private bool isLogoFound;
    private bool isDeveloperFound;
    private bool isPermissionFound;


    // INITIAL SETUP
    private void Start()
    {
        SetPanel(dialoguePanel, true);
        SetPanel(findTheDifferencePanel, false);
        SetPanel(successPanel, false);
        SetPanel(tryAgainPanel, false);

        ResetRound();
    }


    // TIMER
    private void Update()
    {
        if (!isTimerRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerText();
            HandleTimeUp();
            return;
        }

        UpdateTimerText();
    }


    // BUTTON: CONTINUE FROM DIALOGUE
    public void OnContinueClicked()
    {
        SetPanel(dialoguePanel, false);
        SetPanel(successPanel, false);
        SetPanel(tryAgainPanel, false);
        SetPanel(findTheDifferencePanel, true);

        ResetRound();

        isTimerRunning = true;
    }


    // FIND DIFFERENCES
    public void FoundLogo()
    {
        RegisterFound(
            ref isLogoFound,
            logoFound,
            "Logo"
        );
    }


    public void FoundDeveloper()
    {
        RegisterFound(
            ref isDeveloperFound,
            developerFound,
            "Developer"
        );
    }


    public void FoundPermission()
    {
        RegisterFound(
            ref isPermissionFound,
            permissionFound,
            "Permission"
        );
    }


    // REGISTER CORRECT ANSWER
    private void RegisterFound(
        ref bool foundStatus,
        GameObject foundIndicator,
        string differenceName
    )
    {
        if (!isTimerRunning || foundStatus)
            return;

        if (timeRemaining <= 0f)
        {
            HandleTimeUp();
            return;
        }

        foundStatus = true;

        SetPanel(foundIndicator, true);

        Debug.Log(differenceName + " Found!");

        CheckAllFound();
    }


    // CHECK WHETHER ALL DIFFERENCES ARE FOUND
    private void CheckAllFound()
    {
        if (
            isLogoFound &&
            isDeveloperFound &&
            isPermissionFound
        )
        {
            ShowSuccessPanel();
        }
    }


    // SUCCESS PANEL
    private void ShowSuccessPanel()
    {
        isTimerRunning = false;

        SetPanel(findTheDifferencePanel, false);
        SetPanel(tryAgainPanel, false);
        SetPanel(successPanel, true);

        Debug.Log("Level 6 Completed! +10 Security");
    }


    // TIME IS UP
    private void HandleTimeUp()
    {
        isTimerRunning = false;

        if (
            isLogoFound &&
            isDeveloperFound &&
            isPermissionFound
        )
        {
            ShowSuccessPanel();
        }
        else
        {
            ShowTryAgainPanel();
        }
    }


    // TRY AGAIN PANEL
    private void ShowTryAgainPanel()
    {
        isTimerRunning = false;

        SetPanel(findTheDifferencePanel, false);
        SetPanel(successPanel, false);
        SetPanel(tryAgainPanel, true);

        Debug.Log("Time is up! Try again.");
    }


    // BUTTON: TRY AGAIN
    public void RetryLevel()
    {
        SetPanel(tryAgainPanel, false);
        SetPanel(successPanel, false);
        SetPanel(dialoguePanel, false);
        SetPanel(findTheDifferencePanel, true);

        ResetRound();

        isTimerRunning = true;
    }


    // RESET ALL DATA FOR A NEW ROUND
    private void ResetRound()
    {
        isTimerRunning = false;

        isLogoFound = false;
        isDeveloperFound = false;
        isPermissionFound = false;

        SetPanel(logoFound, false);
        SetPanel(developerFound, false);
        SetPanel(permissionFound, false);

        timeRemaining = roundDuration;

        UpdateTimerText();
    }


    // UPDATE TIMER DISPLAY
    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int totalSeconds = Mathf.CeilToInt(timeRemaining);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }


    // HELPER TO ACTIVATE / DEACTIVATE OBJECTS
    private void SetPanel(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
}

