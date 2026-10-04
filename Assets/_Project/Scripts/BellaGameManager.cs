using UnityEngine;
using UnityEngine.UI;

public class BellaGameManager : MonoBehaviour
{
    [Header("========== PANELS ==========")]

    [SerializeField] private GameObject bellaCharacterProfile;
    [SerializeField] private GameObject bellaSMSPanel;

    [SerializeField] private GameObject[] smsDetails = new GameObject[3];

    [SerializeField] private GameObject bellaCorrectPanel;
    [SerializeField] private GameObject bellaWrongPanel;
    [SerializeField] private GameObject bellaSuccessPanel;


    [Header("========== SMS ITEMS ==========")]

    [SerializeField] private Button[] smsItems = new Button[3];


    [Header("========== MAIN PANEL ==========")]

    [SerializeField] private MainPanelManager mainPanelManager;


    private int currentSMS = -1;

    private bool[] completedSMS = new bool[3];


    private void Start()
    {
        SetupBellaGame();
    }


    private void SetupBellaGame()
    {
        bellaCharacterProfile.SetActive(true);

        bellaSMSPanel.SetActive(false);

        bellaCorrectPanel.SetActive(false);
        bellaWrongPanel.SetActive(false);
        bellaSuccessPanel.SetActive(false);


        for (int i = 0; i < smsDetails.Length; i++)
        {
            if (smsDetails[i] != null)
                smsDetails[i].SetActive(false);
        }


        for (int i = 0; i < completedSMS.Length; i++)
        {
            completedSMS[i] = false;
        }


        for (int i = 0; i < smsItems.Length; i++)
        {
            if (smsItems[i] != null)
                smsItems[i].interactable = true;
        }


        currentSMS = -1;
    }


    // =========================================================
    // PROFILE → SMS
    // =========================================================

    public void OpenSMSPanel()
    {
        bellaCharacterProfile.SetActive(false);
        bellaSMSPanel.SetActive(true);

        Debug.Log("Bella SMS Panel opened.");
    }


    // =========================================================
    // OPEN SMS DETAIL
    // =========================================================

    public void OpenSMSDetail(int smsIndex)
    {
        if (smsIndex < 0 || smsIndex >= smsDetails.Length)
        {
            Debug.LogWarning("Invalid SMS index.");
            return;
        }


        if (completedSMS[smsIndex])
        {
            Debug.Log("This SMS is already completed.");
            return;
        }


        currentSMS = smsIndex;


        bellaSMSPanel.SetActive(false);


        for (int i = 0; i < smsDetails.Length; i++)
        {
            if (smsDetails[i] != null)
                smsDetails[i].SetActive(false);
        }


        smsDetails[smsIndex].SetActive(true);

        Debug.Log("Opening SMS " + (smsIndex + 1));
    }


    // =========================================================
    // SAFE BUTTON
    // =========================================================

    public void AnswerSafe()
    {
        if (currentSMS == -1)
        {
            Debug.LogWarning("No SMS selected.");
            return;
        }

        CheckAnswer(true);
    }


    // =========================================================
    // SCAM BUTTON
    // =========================================================

    public void AnswerScam()
    {
        if (currentSMS == -1)
        {
            Debug.LogWarning("No SMS selected.");
            return;
        }

        CheckAnswer(false);
    }


    // =========================================================
    // CHECK ANSWER
    // =========================================================

    private void CheckAnswer(bool playerAnswer)
    {
        bool correct = false;


        // SMS 01 = SCAM
        if (currentSMS == 0)
        {
            correct = playerAnswer == false;
        }


        // SMS 02 = SCAM
        else if (currentSMS == 1)
        {
            correct = playerAnswer == false;
        }


        // SMS 03 = SAFE
        else if (currentSMS == 2)
        {
            correct = playerAnswer == true;
        }


        if (correct)
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }


    // =========================================================
    // CORRECT
    // =========================================================

    private void CorrectAnswer()
    {
        Debug.Log("Correct answer!");


        completedSMS[currentSMS] = true;


        smsDetails[currentSMS].SetActive(false);


        // SMS menjadi gray / disabled
        smsItems[currentSMS].interactable = false;


        bellaCorrectPanel.SetActive(true);
    }


    // =========================================================
    // WRONG
    // =========================================================

    private void WrongAnswer()
    {
        Debug.Log("Wrong answer!");


        smsDetails[currentSMS].SetActive(false);


        // SMS TIDAK di-disable
        // sehingga masih bisa dicoba lagi

        bellaWrongPanel.SetActive(true);
    }


    // =========================================================
    // CORRECT PANEL → CONTINUE
    // =========================================================

    public void CloseCorrectPanel()
    {
        bellaCorrectPanel.SetActive(false);


        if (AllSMSCompleted())
        {
            OpenSuccessPanel();
        }
        else
        {
            bellaSMSPanel.SetActive(true);
        }
    }


    // =========================================================
    // WRONG PANEL → TRY AGAIN
    // =========================================================

    public void CloseWrongPanel()
    {
        bellaWrongPanel.SetActive(false);

        bellaSMSPanel.SetActive(true);
    }


    // =========================================================
    // CHECK ALL SMS
    // =========================================================

    private bool AllSMSCompleted()
    {
        for (int i = 0; i < completedSMS.Length; i++)
        {
            if (!completedSMS[i])
                return false;
        }

        return true;
    }


    // =========================================================
    // SUCCESS
    // =========================================================

    private void OpenSuccessPanel()
    {
        bellaSMSPanel.SetActive(false);

        bellaCorrectPanel.SetActive(false);
        bellaWrongPanel.SetActive(false);

        bellaSuccessPanel.SetActive(true);

        Debug.Log("BELLA COMPLETED!");
    }


    // =========================================================
    // SUCCESS → MAIN MENU
    // =========================================================

    public void BackToMainMenu()
    {
        bellaSuccessPanel.SetActive(false);


        // Bella selesai
        mainPanelManager.LockBella();


        // Mia dibuka
        mainPanelManager.UnlockMia();


        // Kembali ke Main Menu
        mainPanelManager.BackToMainMenu();


        Debug.Log("Bella completed → Mia unlocked.");
    }
}