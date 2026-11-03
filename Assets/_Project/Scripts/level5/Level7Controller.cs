using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level7Controller : MonoBehaviour
{
    [Header("PANELS")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject passwordBuilderPanel;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject tryAgainPanel;


    [Header("FEATURE TOGGLES")]
    [SerializeField] private Toggle lengthToggle;
    [SerializeField] private Toggle uppercaseToggle;
    [SerializeField] private Toggle numbersToggle;
    [SerializeField] private Toggle symbolsToggle;


    [Header("TOGGLE BACKGROUND IMAGES")]
    [SerializeField] private Image lengthToggleBackground;
    [SerializeField] private Image uppercaseToggleBackground;
    [SerializeField] private Image numbersToggleBackground;
    [SerializeField] private Image symbolsToggleBackground;


    [Header("TOGGLE SPRITES")]
    [SerializeField] private Sprite toggleOffSprite;
    [SerializeField] private Sprite toggleOnSprite;


    [Header("PASSWORD METER")]
    [SerializeField] private TMP_Text strengthStatusText;

    [SerializeField] private GameObject[] progressSteps = new GameObject[6];


    private int currentScore;


    // INITIAL SETUP
    private void Start()
    {
        // Register all toggle listeners
        AddToggleListener(lengthToggle);
        AddToggleListener(uppercaseToggle);
        AddToggleListener(numbersToggle);
        AddToggleListener(symbolsToggle);

        // Set initial panels
        SetPanel(dialoguePanel, true);
        SetPanel(passwordBuilderPanel, false);
        SetPanel(successPanel, false);
        SetPanel(tryAgainPanel, false);

        // Reset all features
        ResetPasswordBuilder();
    }


    // REMOVE LISTENERS
    private void OnDestroy()
    {
        RemoveToggleListener(lengthToggle);
        RemoveToggleListener(uppercaseToggle);
        RemoveToggleListener(numbersToggle);
        RemoveToggleListener(symbolsToggle);
    }


    // ADD TOGGLE LISTENER
    private void AddToggleListener(Toggle toggle)
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnFeatureToggleChanged);
        }
    }


    // REMOVE TOGGLE LISTENER
    private void RemoveToggleListener(Toggle toggle)
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnFeatureToggleChanged);
        }
    }


    // WHEN ANY TOGGLE CHANGES
    private void OnFeatureToggleChanged(bool isOn)
    {
        UpdateToggleVisuals();
        UpdatePasswordMeter();
    }


    // UPDATE ON/OFF IMAGES
    private void UpdateToggleVisuals()
    {
        UpdateToggleImage(lengthToggleBackground, lengthToggle);
        UpdateToggleImage(uppercaseToggleBackground, uppercaseToggle);
        UpdateToggleImage(numbersToggleBackground, numbersToggle);
        UpdateToggleImage(symbolsToggleBackground, symbolsToggle);
    }


    private void UpdateToggleImage(Image background, Toggle toggle)
    {
        if (background == null || toggle == null)
            return;

        if (toggle.isOn)
        {
            if (toggleOnSprite != null)
                background.sprite = toggleOnSprite;
        }
        else
        {
            if (toggleOffSprite != null)
                background.sprite = toggleOffSprite;
        }

        background.color = Color.white;
    }


    // CALCULATE PASSWORD SCORE
    private int CalculatePasswordScore()
    {
        int score = 0;

        // Length = 3 points
        if (lengthToggle != null && lengthToggle.isOn)
            score += 3;

        // Uppercase = 1 point
        if (uppercaseToggle != null && uppercaseToggle.isOn)
            score += 1;

        // Numbers = 1 point
        if (numbersToggle != null && numbersToggle.isOn)
            score += 1;

        // Symbols = 1 point
        if (symbolsToggle != null && symbolsToggle.isOn)
            score += 1;

        return Mathf.Clamp(score, 0, 6);
    }


    // UPDATE PASSWORD METER
    private void UpdatePasswordMeter()
    {
        currentScore = CalculatePasswordScore();

        // Show progress bars according to score
        for (int i = 0; i < progressSteps.Length; i++)
        {
            if (progressSteps[i] != null)
            {
                progressSteps[i].SetActive(i < currentScore);
            }
        }

        // Update strength status
        UpdateStrengthStatus();
    }


    // UPDATE WEAK / FAIR / GOOD / STRONG
    private void UpdateStrengthStatus()
    {
        if (strengthStatusText == null)
            return;

        if (currentScore <= 1)
        {
            strengthStatusText.text = "WEAK";
            strengthStatusText.color =
                new Color32(207, 79, 66, 255);
        }
        else if (currentScore <= 3)
        {
            strengthStatusText.text = "FAIR";
            strengthStatusText.color =
                new Color32(218, 155, 65, 255);
        }
        else if (currentScore <= 5)
        {
            strengthStatusText.text = "GOOD";
            strengthStatusText.color =
                new Color32(96, 151, 91, 255);
        }
        else
        {
            strengthStatusText.text = "STRONG";
            strengthStatusText.color =
                new Color32(54, 166, 91, 255);
        }
    }


    // BUTTON: CONTINUE FROM DIALOGUE
    public void OnDialogueContinueClicked()
    {
        SetPanel(dialoguePanel, false);
        SetPanel(passwordBuilderPanel, true);

        SetPanel(successPanel, false);
        SetPanel(tryAgainPanel, false);

        ResetPasswordBuilder();

        Debug.Log("Password Builder Started!");
    }


    // BUTTON: CHECK PASSWORD
    public void OnCheckPasswordClicked()
    {
        currentScore = CalculatePasswordScore();

        Debug.Log("Password Score: " + currentScore + "/6");

        // All four toggles must be ON
        bool allFeaturesEnabled =
            lengthToggle != null && lengthToggle.isOn &&
            uppercaseToggle != null && uppercaseToggle.isOn &&
            numbersToggle != null && numbersToggle.isOn &&
            symbolsToggle != null && symbolsToggle.isOn;

        if (allFeaturesEnabled && currentScore == 6)
        {
            ShowSuccessPanel();
        }
        else
        {
            ShowTryAgainPanel();
        }
    }


    // SUCCESS PANEL
    private void ShowSuccessPanel()
    {
        SetPanel(passwordBuilderPanel, false);
        SetPanel(tryAgainPanel, false);
        SetPanel(successPanel, true);

        Debug.Log("Level 7 Completed! +10 Security");
    }


    // TRY AGAIN PANEL
    private void ShowTryAgainPanel()
    {
        SetPanel(passwordBuilderPanel, false);
        SetPanel(successPanel, false);
        SetPanel(tryAgainPanel, true);

        Debug.Log("Password is not strong enough. Try again!");
    }


    // BUTTON: TRY AGAIN
    public void OnTryAgainClicked()
    {
        SetPanel(tryAgainPanel, false);
        SetPanel(successPanel, false);
        SetPanel(dialoguePanel, false);

        SetPanel(passwordBuilderPanel, true);

        ResetPasswordBuilder();

        Debug.Log("Password Builder Restarted!");
    }


    // RESET ALL TOGGLES AND PROGRESS
    private void ResetPasswordBuilder()
    {
        // Set all toggles to OFF without triggering events
        if (lengthToggle != null)
            lengthToggle.SetIsOnWithoutNotify(false);

        if (uppercaseToggle != null)
            uppercaseToggle.SetIsOnWithoutNotify(false);

        if (numbersToggle != null)
            numbersToggle.SetIsOnWithoutNotify(false);

        if (symbolsToggle != null)
            symbolsToggle.SetIsOnWithoutNotify(false);

        // Return toggle images to OFF
        UpdateToggleVisuals();

        // Empty all progress bars and return status to WEAK
        UpdatePasswordMeter();
    }


    // ACTIVATE / DEACTIVATE PANELS
    private void SetPanel(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
}

