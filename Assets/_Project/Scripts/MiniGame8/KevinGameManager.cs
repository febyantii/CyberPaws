
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KevinGameManager : MonoBehaviour
{
    // ==========================================
    // MAIN PANEL MANAGER
    // ==========================================

    [Header("MAIN PANEL MANAGER")]
    public MainPanelManager8 mainPanelManager;


    // ==========================================
    // KEVIN PANELS
    // ==========================================

    [Header("KEVIN PANELS")]
    public GameObject kevinProfilePanel;
    public GameObject kevinMessagePanel;
    public GameObject kevinPasswordPanel;
    public GameObject kevinBiometricPanel;
    public GameObject kevinNotificationPanel;
    public GameObject kevinCongratulationPanel;
    public GameObject kevinFailedPanel;


    // ==========================================
    // PROFILE PANEL
    // ==========================================

    [Header("PROFILE PANEL")]
    public Button seeMessageButton;


    // ==========================================
    // MESSAGE PANEL
    // ==========================================

    [Header("MESSAGE PANEL")]
    public Button secureButton;


    // ==========================================
    // PASSWORD INPUT
    // ==========================================

    [Header("PASSWORD INPUT")]
    public TMP_InputField passwordInputField;
    public Button checkPasswordButton;


    // ==========================================
    // PASSWORD EYE BUTTON
    // ==========================================

    [Header("PASSWORD EYE BUTTON")]
    public Button eyeButton;
    public Image eyeIcon;

    [Header("EYE SPRITES")]
    public Sprite eyeOpenSprite;
    public Sprite eyeClosedSprite;

    private bool isPasswordVisible = false;


    // ==========================================
    // PASSWORD REQUIREMENTS
    // ==========================================

    [Header("PASSWORD REQUIREMENTS")]
    public TMP_Text lengthRequirement;
    public TMP_Text uppercaseRequirement;
    public TMP_Text numberRequirement;
    public TMP_Text symbolRequirement;


    // ==========================================
    // REQUIREMENT BULLET IMAGES
    // ==========================================

    [Header("REQUIREMENT BULLET IMAGES")]
    public Image bullet1;
    public Image bullet2;
    public Image bullet3;
    public Image bullet4;

    [Header("REQUIREMENT SPRITES")]
    public Sprite bulletInactiveSprite;
    public Sprite bulletActiveSprite;


    // ==========================================
    // PASSWORD SUCCESS POPUP
    // ==========================================

    [Header("PASSWORD SUCCESS POPUP")]
    public GameObject passwordSuccessPopup;


    // ==========================================
    // BIOMETRIC BUTTONS
    // ==========================================

    [Header("BIOMETRIC BUTTONS")]
    public Button enableButton;
    public Button laterButton;
    public Button confirmChoiceButton;


    // ==========================================
    // BIOMETRIC SELECTION IMAGES
    // ==========================================

    [Header("BIOMETRIC SELECTION IMAGES")]
    public GameObject enableImage;
    public GameObject laterImage;


    // ==========================================
    // BIOMETRIC POPUPS
    // ==========================================

    [Header("BIOMETRIC POPUPS")]
    public GameObject biometricSuccessPopup;
    public GameObject biometricErrorPopup;


    // ==========================================
    // NOTIFICATION PANEL
    // ==========================================

    [Header("NOTIFICATION BUTTONS")]
    public Button notificationEnableButton;
    public Button notificationLaterButton;


    // ==========================================
    // FINAL PANELS
    // ==========================================

    [Header("FINAL PANEL BUTTONS")]
    public Button backToMainButton;
    public Button tryAgainButton;


    // ==========================================
    // POPUP SETTINGS
    // ==========================================

    [Header("POPUP SETTINGS")]
    public float popupDuration = 1.5f;


    // ==========================================
    // PRIVATE VARIABLES
    // ==========================================

    private bool enableSelected = false;
    private bool laterSelected = false;

    private Coroutine popupCoroutine;
    private Color enableOriginalColor;
    private Color laterOriginalColor;


    // ==========================================
    // START
    // ==========================================

    private void Start()
    {
        SetupButtons();
        SetupPasswordInput();
        SetupBiometricBackground();

        BeginKevinLevel();
    }


    // ==========================================
    // SETUP BUTTONS
    // ==========================================

    private void SetupButtons()
    {
        // Profile
        if (seeMessageButton != null)
            seeMessageButton.onClick.AddListener(ShowMessagePanel);


        // Message
        if (secureButton != null)
            secureButton.onClick.AddListener(ShowPasswordPanel);


        // Password
        if (checkPasswordButton != null)
            checkPasswordButton.onClick.AddListener(CheckPassword);

        if (eyeButton != null)
            eyeButton.onClick.AddListener(TogglePasswordVisibility);


        // Password Input
        if (passwordInputField != null)
        {
            passwordInputField.onValueChanged.AddListener(
                delegate { UpdatePasswordRequirements(); }
            );
        }


        // Biometric
        if (enableButton != null)
            enableButton.onClick.AddListener(SelectEnable);

        if (laterButton != null)
            laterButton.onClick.AddListener(SelectLater);

        if (confirmChoiceButton != null)
            confirmChoiceButton.onClick.AddListener(ConfirmBiometricChoice);


        // Notification
        if (notificationEnableButton != null)
            notificationEnableButton.onClick.AddListener(ShowCongratulationPanel);

        if (notificationLaterButton != null)
            notificationLaterButton.onClick.AddListener(ShowFailedPanel);


        // Final Panels
        if (backToMainButton != null)
            backToMainButton.onClick.AddListener(BackToMain);

        if (tryAgainButton != null)
            tryAgainButton.onClick.AddListener(BeginKevinLevel);
    }


    // ==========================================
    // SETUP PASSWORD INPUT
    // ==========================================

    private void SetupPasswordInput()
    {
        if (passwordInputField == null)
        {
            Debug.LogError(
                "KevinGameManager: Password Input Field belum dihubungkan!"
            );

            return;
        }

        passwordInputField.contentType =
            TMP_InputField.ContentType.Password;

        passwordInputField.characterValidation =
            TMP_InputField.CharacterValidation.None;

        passwordInputField.lineType =
            TMP_InputField.LineType.SingleLine;

        passwordInputField.ForceLabelUpdate();

        isPasswordVisible = false;

        UpdateEyeIcon();
    }


    // ==========================================
    // PANEL MANAGEMENT
    // ==========================================

    private void HideAllKevinPanels()
    {
        SetPanelActive(kevinProfilePanel, false);
        SetPanelActive(kevinMessagePanel, false);
        SetPanelActive(kevinPasswordPanel, false);
        SetPanelActive(kevinBiometricPanel, false);
        SetPanelActive(kevinNotificationPanel, false);
        SetPanelActive(kevinCongratulationPanel, false);
        SetPanelActive(kevinFailedPanel, false);
    }


    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
            panel.SetActive(active);
    }


    private void ShowOnlyPanel(GameObject targetPanel)
    {
        HideAllKevinPanels();

        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }
        else
        {
            Debug.LogError(
                "KevinGameManager: Panel tujuan belum dihubungkan!"
            );
        }
    }


    // ==========================================
    // START / RESET KEVIN LEVEL
    // ==========================================

    public void BeginKevinLevel()
    {
        // Stop previous popup coroutine
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
            popupCoroutine = null;
        }

        // Reset all challenges
        ResetPassword();
        ResetBiometricChoice();

        // Hide all popups
        HideAllPopups();

        // Reset button states
        if (checkPasswordButton != null)
            checkPasswordButton.interactable = false;

        if (eyeButton != null)
            eyeButton.interactable = true;

        if (confirmChoiceButton != null)
            confirmChoiceButton.interactable = true;

        // Start from Profile
        ShowOnlyPanel(kevinProfilePanel);
    }


    private void HideAllPopups()
    {
        SetPanelActive(passwordSuccessPopup, false);

        SetPanelActive(biometricSuccessPopup, false);
        SetPanelActive(biometricErrorPopup, false);
    }


    // ==========================================
    // PROFILE PANEL
    // ==========================================

    private void ShowMessagePanel()
    {
        ShowOnlyPanel(kevinMessagePanel);
    }


    // ==========================================
    // MESSAGE PANEL
    // ==========================================

    private void ShowPasswordPanel()
    {
        ShowOnlyPanel(kevinPasswordPanel);

        UpdatePasswordRequirements();
    }


    // ==========================================
    // PASSWORD REQUIREMENTS CHECK
    // ==========================================

    private void CheckPasswordRequirements(
        string password,
        out bool hasLength,
        out bool hasUppercase,
        out bool hasNumber,
        out bool hasSymbol)
    {
        hasLength = password.Length >= 6;

        hasUppercase = false;
        hasNumber = false;
        hasSymbol = false;

        foreach (char character in password)
        {
            if (char.IsUpper(character))
                hasUppercase = true;

            if (char.IsDigit(character))
                hasNumber = true;

            if (!char.IsLetterOrDigit(character))
                hasSymbol = true;
        }
    }


    // ==========================================
    // UPDATE PASSWORD REQUIREMENTS
    // ==========================================

    private void UpdatePasswordRequirements()
    {
        if (passwordInputField == null)
            return;

        string password = passwordInputField.text;

        bool hasLength;
        bool hasUppercase;
        bool hasNumber;
        bool hasSymbol;

        CheckPasswordRequirements(
            password,
            out hasLength,
            out hasUppercase,
            out hasNumber,
            out hasSymbol
        );

        UpdateRequirement(lengthRequirement, bullet1, hasLength);

        UpdateRequirement(uppercaseRequirement, bullet2, hasUppercase);

        UpdateRequirement(numberRequirement, bullet3, hasNumber);

        UpdateRequirement(symbolRequirement, bullet4, hasSymbol);


        // Enable Check Button only when all requirements are completed

        bool allRequirementsMet =
            hasLength &&
            hasUppercase &&
            hasNumber &&
            hasSymbol;

        if (checkPasswordButton != null)
            checkPasswordButton.interactable = allRequirementsMet;
    }


    // ==========================================
    // UPDATE REQUIREMENT UI
    // ==========================================

    private void UpdateRequirement(
        TMP_Text requirementText,
        Image bulletImage,
        bool completed)
    {
        if (bulletImage != null)
        {
            bulletImage.sprite = completed
                ? bulletActiveSprite
                : bulletInactiveSprite;
        }

        if (requirementText != null)
        {
            requirementText.color = completed
                ? new Color32(70, 150, 100, 255)
                : new Color32(70, 70, 70, 255);
        }
    }


    // ==========================================
    // RESET PASSWORD
    // ==========================================

    private void ResetPassword()
    {
        if (passwordInputField != null)
        {
            passwordInputField.text = "";

            isPasswordVisible = false;

            passwordInputField.contentType =
                TMP_InputField.ContentType.Password;

            passwordInputField.ForceLabelUpdate();
        }

        UpdateEyeIcon();

        UpdatePasswordRequirements();
    }


    // ==========================================
    // PASSWORD EYE BUTTON
    // ==========================================

    private void TogglePasswordVisibility()
    {
        if (passwordInputField == null)
            return;

        isPasswordVisible = !isPasswordVisible;

        passwordInputField.contentType = isPasswordVisible
            ? TMP_InputField.ContentType.Standard
            : TMP_InputField.ContentType.Password;

        passwordInputField.ForceLabelUpdate();

        UpdateEyeIcon();
    }


    private void UpdateEyeIcon()
    {
        if (eyeIcon == null)
            return;

        eyeIcon.sprite = isPasswordVisible
            ? eyeClosedSprite
            : eyeOpenSprite;
    }


    // ==========================================
    // CHECK PASSWORD
    // ==========================================

    private void CheckPassword()
    {
        if (passwordInputField == null || popupCoroutine != null)
            return;

        string password = passwordInputField.text;

        bool hasLength;
        bool hasUppercase;
        bool hasNumber;
        bool hasSymbol;

        CheckPasswordRequirements(
            password,
            out hasLength,
            out hasUppercase,
            out hasNumber,
            out hasSymbol
        );

        bool isValid =
            hasLength &&
            hasUppercase &&
            hasNumber &&
            hasSymbol;

        // Invalid password cannot proceed
        if (!isValid)
        {
            UpdatePasswordRequirements();
            return;
        }

        // Show Success Popup
        SetPanelActive(passwordSuccessPopup, true);

        popupCoroutine = StartCoroutine(
            PasswordSuccessRoutine()
        );
    }


    // ==========================================
    // PASSWORD SUCCESS ROUTINE
    // ==========================================

    private IEnumerator PasswordSuccessRoutine()
    {
        if (checkPasswordButton != null)
            checkPasswordButton.interactable = false;

        if (eyeButton != null)
            eyeButton.interactable = false;

        yield return new WaitForSeconds(popupDuration);

        SetPanelActive(passwordSuccessPopup, false);

        ShowOnlyPanel(kevinBiometricPanel);

        popupCoroutine = null;
    }

     // ==========================================
    // BIOMETRIC BACKGROUND SETUP
    // ==========================================

    private void SetupBiometricBackground()
    {
        if (enableButton != null && enableButton.targetGraphic != null)
        {
            enableOriginalColor = enableButton.targetGraphic.color;
        }

        if (laterButton != null && laterButton.targetGraphic != null)
        {
            laterOriginalColor = laterButton.targetGraphic.color;
        }

        UpdateBiometricSelection();
    }

    // ==========================================
    // BIOMETRIC SELECTION
    // ==========================================
     private void SelectEnable()
    {
        enableSelected = true;
        laterSelected = false;

        UpdateBiometricSelection();
    }


    private void SelectLater()
    {
        laterSelected = true;
        enableSelected = false;

        UpdateBiometricSelection();
    }

    // ==========================================
    // UPDATE BIOMETRIC IMAGES
    // ==========================================

     private void UpdateBiometricSelection()
    {
        // Enable Background
        if (enableButton != null && enableButton.targetGraphic != null)
        {
            Color color = enableOriginalColor;

            color.a = enableSelected
                ? enableOriginalColor.a
                : 0f;

            enableButton.targetGraphic.color = color;
        }

        // Later Background
        if (laterButton != null && laterButton.targetGraphic != null)
        {
            Color color = laterOriginalColor;

            color.a = laterSelected
                ? laterOriginalColor.a
                : 0f;

            laterButton.targetGraphic.color = color;
        }

        // Child Images always visible
        if (enableImage != null)
            enableImage.SetActive(true);

        if (laterImage != null)
            laterImage.SetActive(true);
    }


    // ==========================================
    // RESET BIOMETRIC CHOICE
    // ==========================================

    private void ResetBiometricChoice()
    {
        enableSelected = false;
        laterSelected = false;

        UpdateBiometricSelection();
    }


    // ==========================================
    // CONFIRM BIOMETRIC CHOICE
    // ==========================================

    private void ConfirmBiometricChoice()
    {
        if (popupCoroutine != null)
            return;

        if (enableSelected)
        {
            // Enable = Success

            SetPanelActive(biometricErrorPopup, false);

            SetPanelActive(biometricSuccessPopup, true);

            popupCoroutine = StartCoroutine(
                BiometricSuccessRoutine()
            );
        }
        else
        {
            // Later or no selection = Error

            SetPanelActive(biometricSuccessPopup, false);

            SetPanelActive(biometricErrorPopup, true);

            popupCoroutine = StartCoroutine(
                BiometricErrorRoutine()
            );
        }
    }


    // ==========================================
    // BIOMETRIC SUCCESS ROUTINE
    // ==========================================

    private IEnumerator BiometricSuccessRoutine()
    {
        if (confirmChoiceButton != null)
            confirmChoiceButton.interactable = false;

        yield return new WaitForSeconds(popupDuration);

        SetPanelActive(biometricSuccessPopup, false);

        ResetBiometricChoice();

        ShowOnlyPanel(kevinNotificationPanel);

        if (confirmChoiceButton != null)
            confirmChoiceButton.interactable = true;

        popupCoroutine = null;
    }


    // ==========================================
    // BIOMETRIC ERROR ROUTINE
    // ==========================================

    private IEnumerator BiometricErrorRoutine()
    {
        if (confirmChoiceButton != null)
            confirmChoiceButton.interactable = false;

        yield return new WaitForSeconds(popupDuration);

        SetPanelActive(biometricErrorPopup, false);

        ResetBiometricChoice();

        ShowOnlyPanel(kevinBiometricPanel);

        if (confirmChoiceButton != null)
            confirmChoiceButton.interactable = true;

        popupCoroutine = null;
    }


    // ==========================================
    // NOTIFICATION PANEL
    // ==========================================

    private void ShowCongratulationPanel()
    {
        ShowOnlyPanel(kevinCongratulationPanel);
    }


    private void ShowFailedPanel()
    {
        ShowOnlyPanel(kevinFailedPanel);
    }


    // ==========================================
    // BACK TO MAIN
    // ==========================================

    private void BackToMain()
    {
        if (mainPanelManager != null)
        {
            mainPanelManager.MarkKevinCompleted();
        }
        else
        {
            Debug.LogError(
                "KevinGameManager: MainPanelManager8 belum dihubungkan!"
            );
        }
    }
}
