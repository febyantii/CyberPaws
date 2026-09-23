using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DropZone : MonoBehaviour,
    IDropHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public bool acceptsSafeLinks;

    private Vector3 originalScale;
    private List<DraggableLink> placedItems = new List<DraggableLink>();

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    // =========================================
    // MOUSE MASUK KE SERVER / TRASH SAAT DRAG
    // =========================================

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DraggableLink.IsDragging)
        {
            transform.localScale = originalScale * 1.08f;
        }
    }

    // =========================================
    // MOUSE KELUAR
    // =========================================

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    // =========================================
    // ITEM DI-DROP
    // =========================================

    public void OnDrop(PointerEventData eventData)
    {
        transform.localScale = originalScale;

        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject == null)
            return;

        DraggableLink link =
            droppedObject.GetComponent<DraggableLink>();

        if (link == null)
            return;

        bool isCorrect =
            link.isSafeLink == acceptsSafeLinks;

        // =====================================
        // BENAR
        // =====================================

        if (isCorrect)
        {
            int index = placedItems.Count;

            // Ukuran link saat sudah masuk target
            Vector2 newSize = new Vector2(320f, 48f);

            // Posisi item di dalam SERVER / TRASH
            float yPosition = -25f - (index * 55f);

            Vector2 position =
                new Vector2(0f, yPosition);

            link.PlaceInZone(
                transform,
                position,
                newSize
            );

            placedItems.Add(link);

            MiniGame2Controller.Instance.CorrectItemPlaced();
        }

        // =====================================
        // SALAH
        // =====================================

        else
        {
            MiniGame2Controller.Instance.ShowWrong();
        }
    }

    // =========================================
    // RESET ISI SERVER / TRASH
    // =========================================

    public void ClearPlacedItems()
    {
        placedItems.Clear();
        transform.localScale = originalScale;
    }
}