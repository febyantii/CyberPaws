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

    void Start()
    {
        ShowDialogue();
    }

    void HideAllPanels()
    {
        DialoguePanel.SetActive(false);
        ScamQuestionPanel.SetActive(false);
        CorrectPanel.SetActive(false);
        WrongPanel.SetActive(false);
    }

    public void ShowDialogue()
    {
        HideAllPanels();
        DialoguePanel.SetActive(true);
    }

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

        // Potong skor acak 2% - 5%
        int penalty = Random.Range(4, 6);
        CityScoreManager.ModifyScore(-penalty);

        Debug.Log($"Pilihan Salah! Security Score berkurang {penalty}%");
    }

    // ==============================
    // PILIH SCAM = BENAR
    // ==============================
    public void ChooseScam()
    {
        HideAllPanels();
        CorrectPanel.SetActive(true);

        // Tambah skor acak 8% - 10%
        int bonus = Random.Range(8, 11);
        CityScoreManager.ModifyScore(bonus);

        Debug.Log($"Pilihan Benar! Security Score bertambah {bonus}%");
    }

    public void TryAgain()
    {
        HideAllPanels();
        ScamQuestionPanel.SetActive(true);
    }

    public void FinishMiniGame()
    {
        // Kosongin aja / tinggalin buat Debug.Log,
        // karena perpindahan scene udah di-handle sama tombol continue kamu!
        Debug.Log("MiniGame 1 Selesai!");
    }
}