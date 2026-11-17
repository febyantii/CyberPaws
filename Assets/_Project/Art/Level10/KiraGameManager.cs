
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class KiraGameManager : MonoBehaviour
{
    [Header("LEVEL 10 PANELS")]
    [SerializeField] private GameObject kiraProfilePanel;
    [SerializeField] private GameObject kiraSecurityPanel;
    [SerializeField] private GameObject kiraMFAPanel;
    [SerializeField] private GameObject kiraVerificationPanel;
    [SerializeField] private GameObject kiraTransactionPanel;
    [SerializeField] private GameObject kiraCongratulationPanel;
    [SerializeField] private GameObject kiraFailedPanel;

    [Header("GAME TIMER")]
    [SerializeField] private TMP_Text gameTimerText;
    [SerializeField] private float timeLimit = 90f;

    [Header("VERIFICATION")]
    [SerializeField] private CredentialDropSlot[] verificationSlots;
    [SerializeField] private CredentialDragItem[] credentialItems;
    [SerializeField] private Button verifyNowButton;

    [Header("MAIN SCENE")]
    [SerializeField] private string mainSceneName = "MainScene";

    private float remainingTime;
    private bool timerRunning;
    private bool levelFinished;
    private bool verificationPassed;

    private void Start()
    {
        // Listen for changes in all verification slots
        if (verificationSlots != null)
        {
            foreach (CredentialDropSlot slot in verificationSlots)
            {
                if (slot != null)
                {
                    slot.OnSlotChanged += UpdateVerifyButton;
                }
            }
        }

        ResetLevel();
    }

    private void OnDestroy()
    {
        if (verificationSlots != null)
        {
            foreach (CredentialDropSlot slot in verificationSlots)
            {
                if (slot != null)
                {
                    slot.OnSlotChanged -= UpdateVerifyButton;
                }
            }
        }
    }

    // =================================
    // TIMER
    // =================================

    private void Update()
    {
        if (!timerRunning || levelFinished)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            UpdateTimerDisplay();
            FailLevel();
            return;
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (gameTimerText == null)
            return;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        gameTimerText.text = $"{minutes:00}:{seconds:00}";
    }

    // =================================
    // PANEL NAVIGATION
    // =================================

    private void ShowPanel(GameObject targetPanel)
    {
        kiraProfilePanel.SetActive(false);
        kiraSecurityPanel.SetActive(false);
        kiraMFAPanel.SetActive(false);
        kiraVerificationPanel.SetActive(false);
        kiraTransactionPanel.SetActive(false);
        kiraCongratulationPanel.SetActive(false);
        kiraFailedPanel.SetActive(false);

        targetPanel.SetActive(true);
    }

    // Profile -> Security
    public void OpenSecurityPanel()
    {
        if (levelFinished) return;

        ShowPanel(kiraSecurityPanel);
    }

    // Security -> MFA
    public void OpenMFAPanel()
    {
        if (levelFinished) return;

        ShowPanel(kiraMFAPanel);
    }

    // MFA -> Verification
    public void OpenVerificationPanel()
    {
        if (levelFinished) return;

        ShowPanel(kiraVerificationPanel);
    }

    // Verification -> Officer Information (MFA)
    public void OpenOfficerInformation()
    {
        if (levelFinished) return;

        ShowPanel(kiraMFAPanel);
    }

    // =================================
    // VERIFICATION BUTTON
    // =================================

    private void UpdateVerifyButton()
    {
        if (verifyNowButton == null || verificationPassed)
            return;

        bool allSlotsFilled = true;

        if (verificationSlots == null ||
            verificationSlots.Length != 3)
        {
            allSlotsFilled = false;
        }
        else
        {
            foreach (CredentialDropSlot slot in verificationSlots)
            {
                if (slot == null || !slot.IsFilled())
                {
                    allSlotsFilled = false;
                    break;
                }
            }
        }

        verifyNowButton.interactable =
            allSlotsFilled && !levelFinished;
    }

    // =================================
    // VERIFY CREDENTIALS
    // =================================

    public void VerifyCredentials()
    {
        if (levelFinished || verificationPassed)
            return;

        if (verificationSlots == null ||
            verificationSlots.Length != 3)
        {
            return;
        }

        // All slots must be filled first
        foreach (CredentialDropSlot slot in verificationSlots)
        {
            if (slot == null || !slot.IsFilled())
                return;
        }

        // Check all credentials
        foreach (CredentialDropSlot slot in verificationSlots)
        {
            if (!slot.IsCorrect())
            {
                FailLevel();
                return;
            }
        }

        // All credentials are correct
        verificationPassed = true;

        if (verifyNowButton != null)
        {
            verifyNowButton.interactable = false;
        }

        // Proceed directly to Transaction
        ShowPanel(kiraTransactionPanel);
    }

    // =================================
    // TRANSACTION
    // =================================

    public void ApproveTransaction()
    {
        if (levelFinished || !verificationPassed)
            return;

        // Approving a suspicious transaction is incorrect
        FailLevel();
    }

    public void BlockTransaction()
    {
        if (levelFinished || !verificationPassed)
            return;

        // Blocking a suspicious transaction is correct
        levelFinished = true;
        timerRunning = false;

        ShowPanel(kiraCongratulationPanel);
    }

    // =================================
    // FAILED
    // =================================

    public void FailLevel()
    {
        if (levelFinished)
            return;

        levelFinished = true;
        timerRunning = false;

        ShowPanel(kiraFailedPanel);
    }

    // =================================
    // RETRY
    // =================================

    public void RetryLevel()
    {
        ResetLevel();
    }

    private void ResetLevel()
    {
        levelFinished = false;
        verificationPassed = false;

        // Reset timer to 90 seconds
        remainingTime = timeLimit;
        timerRunning = true;

        UpdateTimerDisplay();

        // Clear all verification slots
        if (verificationSlots != null)
        {
            foreach (CredentialDropSlot slot in verificationSlots)
            {
                if (slot != null)
                {
                    slot.ClearSlot();
                }
            }
        }

        // Return all credentials to original positions
        if (credentialItems != null)
        {
            foreach (CredentialDragItem item in credentialItems)
            {
                if (item != null)
                {
                    item.ReturnToOrigin();
                }
            }
        }

        // Disable Verify Now until all slots are filled
        if (verifyNowButton != null)
        {
            verifyNowButton.interactable = false;
        }

        // Start from Profile
        ShowPanel(kiraProfilePanel);
    }

    // =================================
    // CONGRATULATION
    // =================================

    public void ContinueToMainScene()
    {
        if (!levelFinished)
            return;

        timerRunning = false;

        SceneManager.LoadScene(
            mainSceneName,
            LoadSceneMode.Single
        );
    }
}
