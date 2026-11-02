using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Level5Controller : MonoBehaviour
{
    // ==========================================
    // PAGE 1 - DIALOGUE
    // ==========================================
    [Header("PAGE 1 - Dialogue")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Button helpButton;


    // ==========================================
    // PAGE 2 - SUSPICIOUS OTP REQUEST
    // ==========================================
    [Header("PAGE 2 - Suspicious OTP Request")]
    [SerializeField] private GameObject suspiciousOTPReqPanel;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button correctButton;
    [SerializeField] private Button falseButton;

    private Coroutine timerCoroutine;
    private const int timerDuration = 8;


    // ==========================================
    // PAGE 3 - CORRECT: DON'T SHARE OTP
    // ==========================================
    [Header("PAGE 3 - Correct Answer")]
    [SerializeField] private GameObject dontShareOTPCorrectPanel;
    [SerializeField] private Button continueButton;


    // ==========================================
    // PAGE 4 - WRONG: GIVE CALLER OTP
    // ==========================================
    [Header("PAGE 4 - Wrong Answer")]
    [SerializeField] private GameObject giveCallerOTPWrongPanel;
    [SerializeField] private Button callerTryAgainButton;


    // ==========================================
    // PAGE 5 - SECURITY VERIFICATION / PIN
    // ==========================================
    [Header("PAGE 5 - Security Verification")]
    [SerializeField] private GameObject securityVerificationPanel;
    [SerializeField] private TMP_Text pinDisplay;
    [SerializeField] private Transform numericKeypad;
    [SerializeField] private Button enterButton;

    private string currentPIN = "";
    private const string correctPIN = "2580";
    private const int pinLength = 4;


    // ==========================================
    // PAGE 6 - SUCCESS
    // ==========================================
    [Header("PAGE 6 - Success")]
    [SerializeField] private GameObject successPanel;


    // ==========================================
    // PAGE 7 - WRONG PIN
    // ==========================================
    [Header("PAGE 7 - Wrong PIN")]
    [SerializeField] private GameObject tryAgainPanel;
    [SerializeField] private Button pinTryAgainButton;


    // ==========================================
    // START
    // ==========================================
    private void Start()
    {
        ConnectMainButtons();
        ConnectKeypadButtons();

        ResetPIN();
        ShowOnlyPanel(dialoguePanel);
    }


    // ==========================================
    // CONNECT MAIN BUTTONS
    // ==========================================
    private void ConnectMainButtons()
    {
        AddButtonListener(helpButton, ShowSuspiciousOTPRequest,
            "HelpButton");

        AddButtonListener(correctButton, ShowCorrectPanel,
            "CorrectButton");

        AddButtonListener(falseButton, ShowCallerWrongPanel,
            "FalseButton");

        AddButtonListener(continueButton, ShowSecurityVerification,
            "ContinueButton");

        AddButtonListener(callerTryAgainButton, ShowSuspiciousOTPRequest,
            "Caller TryAgainButton");

        AddButtonListener(enterButton, CheckPIN,
            "EnterButton");

        AddButtonListener(pinTryAgainButton, ShowSecurityVerification,
            "PIN TryAgainButton");
    }


    private void AddButtonListener(
        Button button,
        UnityAction action,
        string buttonName)
    {
        if (button == null)
        {
            Debug.LogWarning(buttonName + " belum di-assign di Inspector!");
            return;
        }

        button.onClick.AddListener(action);
    }


    // ==========================================
    // SHOW ONLY ONE PANEL
    // ==========================================
    private void ShowOnlyPanel(GameObject panelToShow)
    {
        GameObject[] allPanels =
        {
            dialoguePanel,
            suspiciousOTPReqPanel,
            dontShareOTPCorrectPanel,
            giveCallerOTPWrongPanel,
            securityVerificationPanel,
            successPanel,
            tryAgainPanel
        };

        foreach (GameObject panel in allPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
        else
        {
            Debug.LogError("Panel yang ingin ditampilkan belum di-assign!");
        }
    }


    // ==========================================
    // PAGE 1 -> PAGE 2
    // ==========================================
    private void ShowSuspiciousOTPRequest()
    {
        StopCountdown();

        ShowOnlyPanel(suspiciousOTPReqPanel);

        timerCoroutine = StartCoroutine(CountdownTimer());
    }


    // ==========================================
    // TIMER 8 SECONDS
    // ==========================================
    private IEnumerator CountdownTimer()
    {
        for (int seconds = timerDuration; seconds >= 0; seconds--)
        {
            if (timerText != null)
            {
                timerText.text = "00:" + seconds.ToString("00");
            }

            if (seconds == 0)
            {
                break;
            }

            yield return new WaitForSecondsRealtime(1f);
        }

        // Tampilkan 00:00 sebentar sebelum pindah halaman
        yield return new WaitForSecondsRealtime(0.5f);

        timerCoroutine = null;

        // Waktu habis = dianggap memberikan OTP
        ShowOnlyPanel(giveCallerOTPWrongPanel);
    }


    private void StopCountdown()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }


    // ==========================================
    // PAGE 2 -> PAGE 3
    // CORRECT: DON'T SHARE OTP
    // ==========================================
    private void ShowCorrectPanel()
    {
        StopCountdown();

        ShowOnlyPanel(dontShareOTPCorrectPanel);
    }


    // ==========================================
    // PAGE 2 -> PAGE 4
    // WRONG: GIVE CALLER OTP
    // ==========================================
    private void ShowCallerWrongPanel()
    {
        StopCountdown();

        ShowOnlyPanel(giveCallerOTPWrongPanel);
    }


    // ==========================================
    // PAGE 3 -> PAGE 5
    // CONTINUE TO SECURITY VERIFICATION
    // ==========================================
    private void ShowSecurityVerification()
    {
        StopCountdown();

        ResetPIN();

        ShowOnlyPanel(securityVerificationPanel);
    }


    // ==========================================
    // PAGE 5 - CONNECT NUMERIC KEYPAD
    // ==========================================
    private void ConnectKeypadButtons()
    {
        if (numericKeypad == null)
        {
            Debug.LogError("NumericKeypad belum di-assign di Inspector!");
            return;
        }

        Button[] keypadButtons =
            numericKeypad.GetComponentsInChildren<Button>(true);

        foreach (Button button in keypadButtons)
        {
            string buttonName = button.gameObject.name;

            // BUTTON ANGKA 0-9
            if (buttonName.StartsWith("Button_"))
            {
                string number = buttonName.Substring("Button_".Length);

                if (int.TryParse(number, out int digit))
                {
                    if (digit >= 0 && digit <= 9)
                    {
                        int selectedDigit = digit;

                        button.onClick.AddListener(
                            () => AddPINNumber(selectedDigit)
                        );
                    }
                }
                // BUTTON DELETE
                else if (number == "Delete")
                {
                    button.onClick.AddListener(DeleteLastPINNumber);
                }
                // BUTTON ENTER DI NUMERIC KEYPAD
                else if (number == "Enter")
                {
                    button.onClick.AddListener(CheckPIN);
                }
            }
        }

        Debug.Log("Numeric Keypad berhasil dihubungkan!");
    }


    // ==========================================
    // ADD NUMBER TO PIN
    // ==========================================
    private void AddPINNumber(int number)
    {
        if (securityVerificationPanel == null ||
            !securityVerificationPanel.activeSelf)
        {
            return;
        }

        // Maksimal 4 angka
        if (currentPIN.Length >= pinLength)
        {
            return;
        }

        currentPIN += number.ToString();

        UpdatePINDisplay();

        Debug.Log("Current PIN: " + currentPIN);
    }


    // ==========================================
    // DELETE LAST NUMBER
    // ==========================================
    private void DeleteLastPINNumber()
    {
        if (currentPIN.Length == 0)
        {
            return;
        }

        // Hapus hanya angka terakhir
        currentPIN = currentPIN.Substring(
            0,
            currentPIN.Length - 1
        );

        UpdatePINDisplay();

        Debug.Log("PIN after delete: " + currentPIN);
    }


    // ==========================================
    // UPDATE PIN DISPLAY
    // ==========================================
    private void UpdatePINDisplay()
    {
        if (pinDisplay == null)
        {
            return;
        }

        string[] slots = new string[pinLength];

        for (int i = 0; i < pinLength; i++)
        {
            if (i < currentPIN.Length)
            {
                slots[i] = currentPIN[i].ToString();
            }
            else
            {
                slots[i] = "○";
            }
        }

        pinDisplay.text = string.Join("  ", slots);
    }


    // ==========================================
    // RESET PIN
    // ==========================================
    private void ResetPIN()
    {
        currentPIN = "";

        UpdatePINDisplay();
    }


    // ==========================================
    // CHECK PIN
    // ==========================================
    private void CheckPIN()
    {
        // Harus mengisi 4 digit dahulu
        if (currentPIN.Length != pinLength)
        {
            Debug.LogWarning("Please enter all 4 PIN digits!");
            return;
        }

        if (currentPIN == correctPIN)
        {
            Debug.Log("Correct PIN!");

            // PAGE 6 - SUCCESS
            ShowOnlyPanel(successPanel);
        }
        else
        {
            Debug.Log("Wrong PIN!");

            // PAGE 7 - TRY AGAIN
            ShowOnlyPanel(tryAgainPanel);
        }
    }
}
