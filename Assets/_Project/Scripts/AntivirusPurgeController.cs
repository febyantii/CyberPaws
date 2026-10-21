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

        // --- TAMBAHAN LOGIC SKOR ---
        // Tambah skor acak 8% - 10%
        int bonus = Random.Range(5, 6);
        CityScoreManager.ModifyScore(bonus);
        
        Debug.Log($"Purge Berhasil! Security Score bertambah {bonus}%");
    }

    void ShowTryAgainPanel()
    {
        antivirusPurgePanel.SetActive(false);
        correctPanel.SetActive(false);
        tryAgainPanel.SetActive(true);

        // --- TAMBAHAN LOGIC SKOR ---
        // Potong skor acak 2% - 5%
        int penalty = Random.Range(5, 6);
        CityScoreManager.ModifyScore(-penalty);

        Debug.Log($"Purge Gagal! Security Score berkurang {penalty}%");
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