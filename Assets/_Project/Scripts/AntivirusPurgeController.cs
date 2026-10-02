using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntivirusPurgeController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject dialoguePanel;
    public GameObject antivirusPurgePanel;
    public GameObject correctPanel;
    public GameObject tryAgainPanel;

    [Header("Purge UI")]
    public PurgeHandleResult handleResult;

    void Start()
    {
        dialoguePanel.SetActive(true);
        antivirusPurgePanel.SetActive(false);
        correctPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
    }

    public void OpenAntivirusPurge()
    {
        dialoguePanel.SetActive(false);
        antivirusPurgePanel.SetActive(true);
        correctPanel.SetActive(false);
        tryAgainPanel.SetActive(false);

        if (handleResult != null && handleResult.slider != null)
        {
            handleResult.slider.value = 0f;
        }
    }

    public void CheckResult(bool correct)
    {
        if (correct)
        {
            ShowCorrectPanel();
        }
        else
        {
            ShowTryAgainPanel();
        }
    }

    void ShowCorrectPanel()
    {
        antivirusPurgePanel.SetActive(false);
        correctPanel.SetActive(true);
        tryAgainPanel.SetActive(false);
    }

    void ShowTryAgainPanel()
    {
        antivirusPurgePanel.SetActive(false);
        correctPanel.SetActive(false);
        tryAgainPanel.SetActive(true);
    }

    public void TryAgain()
    {
        tryAgainPanel.SetActive(false);
        correctPanel.SetActive(false);
        antivirusPurgePanel.SetActive(true);

        if (handleResult != null && handleResult.slider != null)
        {
            handleResult.slider.value = 0f;
        }
    }

    public void PressOK()
    {
        correctPanel.SetActive(false);
    }
}