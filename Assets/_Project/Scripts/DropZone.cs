using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour,
    IDropHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public bool acceptsSafeLinks;

    // Tempat link akan dimasukkan
    public Transform contentParent;

    private Vector3 originalScale;

    private List<DraggableLink> placedItems =
        new List<DraggableLink>();

    // Menyimpan hasil setiap link
    // true  = benar
    // false = salah
    private static Dictionary<DraggableLink, bool> placedResults =
        new Dictionary<DraggableLink, bool>();

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    // =========================================
    // HOVER DROP ZONE
    // =========================================

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DraggableLink.IsDragging)
        {
            transform.localScale = originalScale * 1.03f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    // =========================================
    // DROP LINK
    // =========================================

    public void OnDrop(PointerEventData eventData)
    {
        transform.localScale = originalScale;

        GameObject droppedObject =
            eventData.pointerDrag;

        if (droppedObject == null)
            return;

        DraggableLink link =
            droppedObject.GetComponent<DraggableLink>();

        if (link == null)
            return;

        // Cek apakah link benar untuk zone ini
        bool isCorrect =
            link.isSafeLink == acceptsSafeLinks;

        // =========================================
        // TENTUKAN PARENT
        // =========================================

        Transform targetParent = contentParent;

        if (targetParent == null)
        {
            targetParent = transform;
        }

        // =========================================
        // MASUKKAN LINK KE ZONE
        // BENAR ATAU SALAH TETAP MASUK
        // =========================================

        link.PlaceInZone(
            this,
            targetParent
        );

        if (!placedItems.Contains(link))
        {
            placedItems.Add(link);
        }

        // Simpan hasil link
        placedResults[link] = isCorrect;

        Debug.Log(
            "Link masuk: " +
            placedResults.Count +
            "/" +
            MiniGame2Controller.Instance.TotalLinks
        );

        // =========================================
        // CEK APAKAH SEMUA SUDAH MASUK
        // =========================================

        CheckAllLinksPlaced();
    }

    // =========================================
    // CEK SEMUA LINK
    // =========================================

    private void CheckAllLinksPlaced()
    {
        int totalLinks =
            MiniGame2Controller.Instance.TotalLinks;

        // Safety
        if (totalLinks <= 0)
            return;

        // =========================================
        // BELUM SEMUA MASUK
        // =========================================

        if (placedResults.Count < totalLinks)
        {
            Debug.Log(
                "Belum selesai. " +
                placedResults.Count +
                "/" +
                totalLinks
            );

            return;
        }

        // =========================================
        // SEMUA SUDAH MASUK
        // SEKARANG CEK HASIL
        // =========================================

        bool allCorrect = true;

        foreach (bool result in placedResults.Values)
        {
            if (!result)
            {
                allCorrect = false;
                break;
            }
        }

        // =========================================
        // ADA YANG SALAH
        // =========================================

        if (!allCorrect)
        {
            MiniGame2Controller.Instance.ShowWrong();
        }

        // =========================================
        // SEMUANYA BENAR
        // =========================================

        else
        {
            MiniGame2Controller.Instance.ShowCorrect();
        }
    }

    // =========================================
    // HAPUS LINK DARI LIST
    // =========================================

    public void RemovePlacedItem(DraggableLink link)
    {
        placedItems.Remove(link);
    }

    // =========================================
    // HAPUS HASIL LINK
    // =========================================

    public static void NotifyLinkRemoved(
        DraggableLink link)
    {
        if (placedResults.ContainsKey(link))
        {
            placedResults.Remove(link);
        }
    }

    // =========================================
    // RESET HASIL SEMUA LINK
    // =========================================

    public static void ResetAllResults()
    {
        placedResults.Clear();
    }

    // =========================================
    // RESET ZONE
    // =========================================

    public void ClearPlacedItems()
    {
        placedItems.Clear();

        transform.localScale = originalScale;
    }
}