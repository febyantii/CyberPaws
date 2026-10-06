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

    private DraggableLink[] allLinks;

    // =========================================
    // JUMLAH SEMUA LINK
    // =========================================

    public int TotalLinks
    {
        get
        {
            return allLinks != null ? allLinks.Length : 0;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Aktifkan sebentar supaya semua DraggableLink menjalankan Awake()
        InspeksiPanel.SetActive(true);

        // Ambil semua DraggableLink
        allLinks =
            InspeksiPanel.GetComponentsInChildren<DraggableLink>(true);

        Debug.Log("Total link: " + TotalLinks);

        // Tampilkan Dialogue dan sembunyikan panel lainnya
        ShowDialogue();
    }

    // =========================================
    // AWAL GAME
    // =========================================

    public void ShowDialogue()
    {
        DialoguePanel.SetActive(true);
        InspeksiPanel.SetActive(false);
        PageBenar.SetActive(false);
        PageSalah.SetActive(false);

        ResetGame();
    }

    // =========================================
    // KLIK INSPEKSI
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
    // SALAH
    // =========================================

    public void ShowWrong()
    {
        InspeksiPanel.SetActive(false);
        PageSalah.SetActive(true);

        // --- TAMBAHAN LOGIC SKOR ---
        // Potong skor acak 2% - 5%
        int penalty = Random.Range(1, 5);
        CityScoreManager.ModifyScore(-penalty);
        
        Debug.Log($"Ada link yang salah! Security Score berkurang {penalty}%");
    }

    // =========================================
    // SEMUA BENAR
    // =========================================

    public void ShowCorrect()
    {
        InspeksiPanel.SetActive(false);
        PageBenar.SetActive(true);

        // --- TAMBAHAN LOGIC SKOR ---
        // Tambah skor acak 8% - 10%
        int bonus = Random.Range(8, 11);
        CityScoreManager.ModifyScore(bonus);

        Debug.Log($"Semua link benar! Security Score bertambah {bonus}%");
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
    // RESET GAME
    // =========================================

    private void ResetGame()
    {
        // Reset data hasil drop
        DropZone.ResetAllResults();

        // Reset daftar item dalam zone
        if (ServerDropZone != null)
        {
            ServerDropZone.ClearPlacedItems();
        }

        if (TrashDropZone != null)
        {
            TrashDropZone.ClearPlacedItems();
        }

        // Kembalikan semua link
        // ke posisi awal
        if (allLinks != null)
        {
            foreach (DraggableLink link in allLinks)
            {
                if (link != null)
                {
                    link.ResetPosition();
                }
            }
        }
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