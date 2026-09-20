using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame1UI : MonoBehaviour
{
    // Panel yang ada di Hierarchy
    public GameObject DialoguePanel;
    public GameObject ScamQuestionPanel;
    public GameObject CorrectPanel;
    public GameObject WrongPanel;
    public GameObject CompletePanel;

    void Start()
    {
        ShowDialogue();
    }

    // Mematikan semua panel
    void HideAllPanels()
    {
        DialoguePanel.SetActive(false);
        ScamQuestionPanel.SetActive(false);
        CorrectPanel.SetActive(false);
        WrongPanel.SetActive(false);
        CompletePanel.SetActive(false);
    }

    // ==============================
    // AWAL GAME
    // ==============================

    public void ShowDialogue()
    {
        HideAllPanels();
        DialoguePanel.SetActive(true);
    }

    // ==============================
    // BUKA PESAN
    // ==============================

    public void OpenMessage()
    {
        HideAllPanels();
        ScamQuestionPanel.SetActive(true);
    }

    // ==============================
    // PILIH LEGIT = SALAH
    // ==============================

    public void ChooseLegit()
    {
        HideAllPanels();
        WrongPanel.SetActive(true);
    }

    // ==============================
    // PILIH SCAM = BENAR
    // ==============================

    public void ChooseScam()
    {
        HideAllPanels();
        CorrectPanel.SetActive(true);
    }

    // ==============================
    // COBA LAGI
    // ==============================

    public void TryAgain()
    {
        HideAllPanels();
        ScamQuestionPanel.SetActive(true);
    }

    // ==============================
    // LANJUT KE COMPLETE
    // ==============================

    public void GoToComplete()
    {
        HideAllPanels();
        CompletePanel.SetActive(true);
    }

    // ==============================
    // SELESAI
    // ==============================

    public void FinishMiniGame()
    {
        Debug.Log("MiniGame 1 selesai!");
    }
}