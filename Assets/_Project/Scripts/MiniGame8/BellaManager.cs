
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BellaManager : MonoBehaviour
{
    [Serializable]
    public class SlotPair
    {
        public Button safeSlot;
        public Button scamSlot;

        [HideInInspector]
        public bool isSelected;
    }

    [Header("Panels")]
    public GameObject bellaProfilePanel;
    public GameObject bellaMessagePanel;
    public GameObject bellaWebsitePanel;
    public GameObject bellaCongratulationPanel;
    public GameObject bellaFailedPanel;

    [Header("Navigation Buttons")]
    public Button seeMessageButton;
    public Button checkVoucherButton;
    public Button submitButton;
    public Button restartButton;
    public Button backToGameButton;
    public Button tryAgainButton;

    [Header("Website Slots")]
    public SlotPair[] slotPairs = new SlotPair[5];
    public Button[] dummySlots = new Button[18];

    [Header("Counter Text")]
    public TMP_Text foundText;
    public TMP_Text mistakesText;

    [Header("Instruction")]
    public TMP_Text websiteInstruction;

    [Header("Scene Settings")]
    public string mainGameSceneName;

    [Header("Slot Effect")]
    [Range(0f, 1f)]
    public float effectAlpha = 0.3f;

    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    private int foundCount = 0;
    private int mistakesCount = 0;

    private const int MAX_FOUND = 5;
    private const int MAX_MISTAKES = 5;

    private bool isGameOver = false;
    private bool allFound = false;

    private Color[][] originalPairColors;
    private Color[] originalDummyColors;

    private void Start()
    {
        SaveOriginalColors();
        SetupButtons();
        BeginLevel();
    }

    // =========================================
    // SETUP BUTTONS
    // =========================================

    private void SetupButtons()
    {
        if (seeMessageButton != null)
            seeMessageButton.onClick.AddListener(ShowMessagePanel);

        if (checkVoucherButton != null)
            checkVoucherButton.onClick.AddListener(ShowWebsitePanel);

        if (submitButton != null)
            submitButton.onClick.AddListener(SubmitChallenge);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartChallenge);

        if (backToGameButton != null)
            backToGameButton.onClick.AddListener(BackToGame);

        if (tryAgainButton != null)
            tryAgainButton.onClick.AddListener(TryAgain);

        for (int i = 0; i < slotPairs.Length; i++)
        {
            int index = i;

            if (slotPairs[i] == null)
                continue;

            if (slotPairs[i].safeSlot != null)
            {
                PrepareSlotButton(slotPairs[i].safeSlot);

                slotPairs[i].safeSlot.onClick.AddListener(
                    () => SelectSlot(index, true)
                );
            }

            if (slotPairs[i].scamSlot != null)
            {
                PrepareSlotButton(slotPairs[i].scamSlot);

                slotPairs[i].scamSlot.onClick.AddListener(
                    () => SelectSlot(index, false)
                );
            }
        }

        for (int i = 0; i < dummySlots.Length; i++)
        {
            int index = i;

            if (dummySlots[i] == null)
                continue;

            PrepareSlotButton(dummySlots[i]);

            dummySlots[i].onClick.AddListener(
                () => SelectDummySlot(index)
            );
        }
    }

    private void PrepareSlotButton(Button button)
    {
        if (button == null)
            return;

        button.transition = Selectable.Transition.None;
    }

    // =========================================
    // SAVE ORIGINAL COLORS
    // =========================================

    private void SaveOriginalColors()
    {
        originalPairColors = new Color[slotPairs.Length][];

        for (int i = 0; i < slotPairs.Length; i++)
        {
            originalPairColors[i] = new Color[2];

            if (slotPairs[i] == null)
                continue;

            originalPairColors[i][0] =
                GetButtonColor(slotPairs[i].safeSlot);

            originalPairColors[i][1] =
                GetButtonColor(slotPairs[i].scamSlot);
        }

        originalDummyColors = new Color[dummySlots.Length];

        for (int i = 0; i < dummySlots.Length; i++)
        {
            originalDummyColors[i] =
                GetButtonColor(dummySlots[i]);
        }
    }

    private Color GetButtonColor(Button button)
    {
        if (button == null || button.targetGraphic == null)
            return Color.white;

        return button.targetGraphic.color;
    }

    // =========================================
    // LEVEL FLOW
    // =========================================

    private void BeginLevel()
    {
        foundCount = 0;
        mistakesCount = 0;

        isGameOver = false;
        allFound = false;

        ResetSlotStates();
        UpdateCounterText();
        ResetInstruction();

        if (restartButton != null)
            restartButton.interactable = true;

        ShowProfilePanel();
    }

    private void ShowProfilePanel()
    {
        SetActivePanel(bellaProfilePanel);
    }

    private void ShowMessagePanel()
    {
        if (isGameOver)
            return;

        SetActivePanel(bellaMessagePanel);
    }

    private void ShowWebsitePanel()
    {
        if (isGameOver)
            return;

        SetActivePanel(bellaWebsitePanel);
    }

    private void SetActivePanel(GameObject activePanel)
    {
        if (bellaProfilePanel != null)
            bellaProfilePanel.SetActive(false);

        if (bellaMessagePanel != null)
            bellaMessagePanel.SetActive(false);

        if (bellaWebsitePanel != null)
            bellaWebsitePanel.SetActive(false);

        if (bellaCongratulationPanel != null)
            bellaCongratulationPanel.SetActive(false);

        if (bellaFailedPanel != null)
            bellaFailedPanel.SetActive(false);

        if (activePanel != null)
            activePanel.SetActive(true);
    }

    // =========================================
    // SAFE / SCAM SLOT
    // =========================================

    private void SelectSlot(int index, bool isSafe)
    {
        if (isGameOver || allFound)
            return;

        if (index < 0 || index >= slotPairs.Length)
            return;

        SlotPair pair = slotPairs[index];

        if (pair == null || pair.isSelected)
            return;

        pair.isSelected = true;

        Button selectedButton = isSafe
            ? pair.safeSlot
            : pair.scamSlot;

        // Green effect
        ApplySlotEffect(selectedButton, correctColor);

        // Disable both buttons in the same pair
        if (pair.safeSlot != null)
            pair.safeSlot.interactable = false;

        if (pair.scamSlot != null)
            pair.scamSlot.interactable = false;

        foundCount++;

        UpdateCounterText();

        // All 5 pairs have been found
        if (foundCount >= MAX_FOUND)
        {
            foundCount = MAX_FOUND;
            allFound = true;

            DisableAllSlots();

            if (websiteInstruction != null)
            {
                websiteInstruction.text =
                    "You've already found all 5 elements! Submit your answer to continue.";
            }

            if (restartButton != null)
                restartButton.interactable = false;
        }
    }

    // =========================================
    // DUMMY SLOT
    // =========================================

    private void SelectDummySlot(int index)
    {
        if (isGameOver || allFound)
            return;

        if (index < 0 || index >= dummySlots.Length)
            return;

        Button dummy = dummySlots[index];

        if (dummy == null || !dummy.interactable)
            return;

        // Red effect
        ApplySlotEffect(dummy, wrongColor);

        // Disable clicked dummy
        dummy.interactable = false;

        mistakesCount++;

        UpdateCounterText();

        if (mistakesCount > MAX_MISTAKES)
        {
            FailChallenge();
        }
    }

    // =========================================
    // SLOT COLOR EFFECT
    // =========================================

    private void ApplySlotEffect(Button button, Color effectColor)
    {
        if (button == null || button.targetGraphic == null)
            return;

        Color originalColor = button.targetGraphic.color;

        effectColor.a = effectAlpha;

        button.targetGraphic.color = Color.Lerp(
            originalColor,
            effectColor,
            effectAlpha
        );
    }

    // =========================================
    // DISABLE ALL SLOTS
    // =========================================

    private void DisableAllSlots()
    {
        foreach (SlotPair pair in slotPairs)
        {
            if (pair == null)
                continue;

            if (pair.safeSlot != null)
                pair.safeSlot.interactable = false;

            if (pair.scamSlot != null)
                pair.scamSlot.interactable = false;
        }

        foreach (Button dummy in dummySlots)
        {
            if (dummy != null)
                dummy.interactable = false;
        }
    }

    // =========================================
    // SUBMIT
    // =========================================

    private void SubmitChallenge()
    {
        if (isGameOver)
            return;

        if (foundCount >= MAX_FOUND)
        {
            CompleteChallenge();
        }
        else
        {
            if (websiteInstruction != null)
            {
                websiteInstruction.text =
                    "Keep looking! You haven't found all 5 elements yet.";
            }
        }
    }

    // =========================================
    // RESTART CHALLENGE
    // =========================================

    private void RestartChallenge()
    {
        if (isGameOver || allFound)
            return;

        foundCount = 0;
        mistakesCount = 0;

        isGameOver = false;
        allFound = false;

        ResetSlotStates();
        UpdateCounterText();
        ResetInstruction();

        if (restartButton != null)
            restartButton.interactable = true;

        SetActivePanel(bellaWebsitePanel);
    }

    // =========================================
    // RESET SLOT STATES
    // =========================================

    private void ResetSlotStates()
    {
        for (int i = 0; i < slotPairs.Length; i++)
        {
            SlotPair pair = slotPairs[i];

            if (pair == null)
                continue;

            pair.isSelected = false;

            if (pair.safeSlot != null)
            {
                pair.safeSlot.interactable = true;

                if (pair.safeSlot.targetGraphic != null)
                {
                    pair.safeSlot.targetGraphic.color =
                        originalPairColors[i][0];
                }
            }

            if (pair.scamSlot != null)
            {
                pair.scamSlot.interactable = true;

                if (pair.scamSlot.targetGraphic != null)
                {
                    pair.scamSlot.targetGraphic.color =
                        originalPairColors[i][1];
                }
            }
        }

        for (int i = 0; i < dummySlots.Length; i++)
        {
            if (dummySlots[i] == null)
                continue;

            dummySlots[i].interactable = true;

            if (dummySlots[i].targetGraphic != null)
            {
                dummySlots[i].targetGraphic.color =
                    originalDummyColors[i];
            }
        }
    }

    // =========================================
    // COUNTER
    // =========================================

    private void UpdateCounterText()
    {
        if (foundText != null)
        {
            foundText.text =
                "FOUND " + foundCount + "/" + MAX_FOUND;
        }

        if (mistakesText != null)
        {
            mistakesText.text =
                "MISTAKES " + mistakesCount + "/" + MAX_MISTAKES;
        }
    }

    // =========================================
    // INSTRUCTION
    // =========================================

    private void ResetInstruction()
    {
        if (websiteInstruction != null)
        {
            websiteInstruction.text =
                "Find all 5 suspicious and safe elements on the website.";
        }
    }

    // =========================================
    // CONGRATULATION
    // =========================================

    private void CompleteChallenge()
    {
        isGameOver = true;

        SetActivePanel(bellaCongratulationPanel);
    }

    // =========================================
    // FAILED
    // =========================================

    private void FailChallenge()
    {
        isGameOver = true;

        SetActivePanel(bellaFailedPanel);
    }

    // =========================================
    // TRY AGAIN
    // =========================================

    private void TryAgain()
    {
        BeginLevel();
    }

    // =========================================
    // BACK TO MAIN GAME
    // =========================================

    private void BackToGame()
    {
        if (string.IsNullOrEmpty(mainGameSceneName))
        {
            Debug.LogWarning(
                "Main Game Scene Name has not been assigned in BellaManager Inspector."
            );

            return;
        }

        SceneManager.LoadScene(mainGameSceneName);
    }
}
