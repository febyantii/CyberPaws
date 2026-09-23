using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame2Controller : MonoBehaviour
{
    public static MiniGame2Controller Instance;

    [Header("Pages")]
    public GameObject DialoguePanel;
    public GameObject InspeksiPanel;
    public GameObject PageBenar;
    public GameObject PageSalah;

    [Header("Drop Zones")]
    public DropZone ServerDropZone;
    public DropZone TrashDropZone;

    private int correctItems = 0;

    private DraggableLink[] allLinks;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Ambil semua link yang ada di Inspection Panel
        allLinks =
            InspeksiPanel.GetComponentsInChildren<DraggableLink>(true);

        ShowDialogue();
    }

    // =========================================
    // AWAL
    // =========================================

    public void ShowDialogue()
    {
        DialoguePanel.SetActive(true);
        InspeksiPanel.SetActive(false);
        PageBenar.SetActive(false);
        PageSalah.SetActive(false);

        correctItems = 0;
    }

    // =========================================
    // KLIK "INSPEKSI PESAN"
    // =========================================

    public void OpenInspection()
    {
        DialoguePanel.SetActive(false);
        InspeksiPanel.SetActive(true);
        PageBenar.SetActive(false);
        PageSalah.SetActive(false);

        ResetGame();
    }

    // =========================================
    // ITEM BENAR
    // =========================================

    public void CorrectItemPlaced()
    {
        correctItems++;

        Debug.Log(
            "Item benar: " +
            correctItems +
            "/4"
        );

        // Kalau semua 4 sudah benar
        if (correctItems >= 4)
        {
            ShowCorrect();
        }
    }

    // =========================================
    // SALAH DROP
    // =========================================

    public void ShowWrong()
    {
        InspeksiPanel.SetActive(false);
        PageSalah.SetActive(true);

        Debug.Log("Jawaban salah!");
    }

    // =========================================
    // PAGE BENAR
    // =========================================

    public void ShowCorrect()
    {
        InspeksiPanel.SetActive(false);
        PageBenar.SetActive(true);

        Debug.Log("Semua jawaban benar!");
    }

    // =========================================
    // KLIK TRY AGAIN
    // =========================================

    public void Retry()
    {
        PageSalah.SetActive(false);
        InspeksiPanel.SetActive(true);

        ResetGame();
    }

    // =========================================
    // RESET SEMUA ITEM
    // =========================================

    private void ResetGame()
    {
        correctItems = 0;

        foreach (DraggableLink link in allLinks)
        {
            link.ResetPosition();
        }

        ServerDropZone.ClearPlacedItems();
        TrashDropZone.ClearPlacedItems();
    }

    // =========================================
    // KLIK OK DI PAGE BENAR
    // =========================================

    public void FinishLevel()
    {
        PageBenar.SetActive(false);

        Debug.Log("LEVEL 2 SELESAI!");
    }
}