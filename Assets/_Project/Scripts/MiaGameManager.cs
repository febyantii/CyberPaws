using UnityEngine;

public class MiaGameManager : MonoBehaviour
{
    [Header("Page Panels")]
    public GameObject characterProfilePanel;
    public GameObject smsPanel;
    public GameObject miaGamePanel;
    public GameObject congratulationPanel;

    private MiaLinkCards[] links;

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Kondisi awal
        if (characterProfilePanel != null)
            characterProfilePanel.SetActive(true);

        if (smsPanel != null)
            smsPanel.SetActive(false);

        if (miaGamePanel != null)
            miaGamePanel.SetActive(false);

        if (congratulationPanel != null)
            congratulationPanel.SetActive(false);

        // Cari semua LinkCard
        RefreshLinks();

        Debug.Log("Mia Game Manager siap.");
    }

    // =========================================================
    // REFRESH LINKS
    // =========================================================

    private void RefreshLinks()
    {
        links = FindObjectsOfType<MiaLinkCards>();

        Debug.Log(
            "Jumlah MiaLinkCards ditemukan: " +
            (links != null ? links.Length : 0)
        );
    }

    // =========================================================
    // CHARACTER PROFILE -> SMS
    // =========================================================

    public void ContinueFromCharacterProfile()
    {
        if (characterProfilePanel != null)
            characterProfilePanel.SetActive(false);

        if (smsPanel != null)
            smsPanel.SetActive(true);

        Debug.Log("Masuk ke SMS Panel.");
    }

    // =========================================================
    // SMS -> MIA GAME
    // =========================================================

    public void HelpMia()
    {
        if (smsPanel != null)
            smsPanel.SetActive(false);

        if (miaGamePanel != null)
            miaGamePanel.SetActive(true);

        Debug.Log("Masuk ke Mia Game Panel.");
    }

    // =========================================================
    // DONE BUTTON
    // =========================================================

    public void CheckAnswers()
    {
        RefreshLinks();

        if (links == null || links.Length == 0)
        {
            Debug.LogWarning(
                "Tidak ada MiaLinkCards ditemukan."
            );

            return;
        }

        // =====================================================
        // STEP 1
        // PASTIKAN SEMUA LINK SUDAH MASUK SAFE / SCAM
        // =====================================================

        foreach (MiaLinkCards link in links)
        {
            if (link == null)
                continue;

            if (link.IsInAllBox())
            {
                Debug.Log(
                    "Masih ada link di All Box. " +
                    "Pindahkan semua link ke Safe atau Scam."
                );

                return;
            }
        }

        // =====================================================
        // STEP 2
        // CEK SEMUA JAWABAN
        // =====================================================

        bool allCorrect = true;

        foreach (MiaLinkCards link in links)
        {
            if (link == null)
                continue;

            // -------------------------------------------------
            // BENAR
            // -------------------------------------------------

            if (link.IsCorrect())
            {
                Debug.Log(
                    link.gameObject.name +
                    " = BENAR"
                );

                // Jangan lakukan apa-apa.
                // Card tetap berada di Safe / Scam.
            }

            // -------------------------------------------------
            // SALAH
            // -------------------------------------------------

            else
            {
                Debug.Log(
                    link.gameObject.name +
                    " = SALAH -> kembali ke All Box"
                );

                allCorrect = false;

                // Hanya card yang salah yang dikembalikan
                link.ReturnToOriginalAllSlot();
            }
        }

        // =====================================================
        // STEP 3
        // HASIL
        // =====================================================

        if (allCorrect)
        {
            ShowCongratulation();
        }
        else
        {
            Debug.Log(
                "Masih ada jawaban yang salah. " +
                "Card yang salah sudah dikembalikan ke All Box."
            );
        }
    }

    // =========================================================
    // CONGRATULATION
    // =========================================================

    private void ShowCongratulation()
    {
        if (miaGamePanel != null)
            miaGamePanel.SetActive(false);

        if (congratulationPanel != null)
            congratulationPanel.SetActive(true);

        Debug.Log(
            "SEMUA JAWABAN BENAR!"
        );
    }
}
