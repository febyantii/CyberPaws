using UnityEngine;

public class BellaScamGame : MonoBehaviour
{
    [Header("Main Pages")]
    public GameObject backgroundImage;
    public GameObject smsPanel;

    [Header("Message Details")]
    public GameObject smsDetails1;
    public GameObject smsDetails2;
    public GameObject smsDetails3;

    [Header("Result Panels")]
    public GameObject correctPanel;
    public GameObject wrongPanel;
    public GameObject successPanel;

    private int currentMessage = 0;

    void Start()
    {
        ShowPage(backgroundImage);

        smsPanel.SetActive(false);
        smsDetails1.SetActive(false);
        smsDetails2.SetActive(false);
        smsDetails3.SetActive(false);

        correctPanel.SetActive(false);
        wrongPanel.SetActive(false);
        successPanel.SetActive(false);
    }

    // =========================
    // PAGE 1
    // =========================

    public void OpenSMS()
    {
        backgroundImage.SetActive(false);
        smsPanel.SetActive(true);
    }

    // =========================
    // SMS PANEL
    // =========================

    public void OpenMessage1()
    {
        currentMessage = 1;

        smsPanel.SetActive(false);
        smsDetails1.SetActive(true);
    }

    public void OpenMessage2()
    {
        currentMessage = 2;

        smsPanel.SetActive(false);
        smsDetails2.SetActive(true);
    }

    public void OpenMessage3()
    {
        currentMessage = 3;

        smsPanel.SetActive(false);
        smsDetails3.SetActive(true);
    }

    // =========================
    // MESSAGE 1
    // Correct = SCAM
    // =========================

    public void Message1Scam()
    {
        ShowCorrect();
    }

    public void Message1Safe()
    {
        ShowWrong();
    }

    // =========================
    // MESSAGE 2
    // Correct = SCAM
    // =========================

    public void Message2Scam()
    {
        ShowCorrect();
    }

    public void Message2Safe()
    {
        ShowWrong();
    }

    // =========================
    // MESSAGE 3
    // Correct = SAFE
    // =========================

    public void Message3Scam()
    {
        ShowWrong();
    }

    public void Message3Safe()
    {
        ShowCorrect();
    }

    // =========================
    // CORRECT
    // =========================

    void ShowCorrect()
    {
        HideAllDetails();

        correctPanel.SetActive(true);
    }

    // =========================
    // WRONG
    // =========================

    void ShowWrong()
    {
        HideAllDetails();

        wrongPanel.SetActive(true);
    }

    // =========================
    // CONTINUE AFTER CORRECT
    // =========================

    public void ContinueAfterCorrect()
    {
        correctPanel.SetActive(false);

        if (currentMessage == 1)
        {
            smsPanel.SetActive(true);
        }
        else if (currentMessage == 2)
        {
            smsPanel.SetActive(true);
        }
        else if (currentMessage == 3)
        {
            successPanel.SetActive(true);
        }
    }

    // =========================
    // TRY AGAIN
    // =========================

    public void TryAgain()
    {
        wrongPanel.SetActive(false);

        if (currentMessage == 1)
        {
            smsDetails1.SetActive(true);
        }
        else if (currentMessage == 2)
        {
            smsDetails2.SetActive(true);
        }
        else if (currentMessage == 3)
        {
            smsDetails3.SetActive(true);
        }
    }

    // =========================
    // HIDE ALL MESSAGE DETAILS
    // =========================

    void HideAllDetails()
    {
        smsDetails1.SetActive(false);
        smsDetails2.SetActive(false);
        smsDetails3.SetActive(false);
    }

    // =========================
    // HIDE EVERYTHING
    // =========================

    void ShowPage(GameObject page)
    {
        backgroundImage.SetActive(false);
        smsPanel.SetActive(false);

        smsDetails1.SetActive(false);
        smsDetails2.SetActive(false);
        smsDetails3.SetActive(false);

        correctPanel.SetActive(false);
        wrongPanel.SetActive(false);
        successPanel.SetActive(false);

        page.SetActive(true);
    }
}