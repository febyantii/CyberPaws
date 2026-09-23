using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DraggableLink : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public bool isSafeLink;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas rootCanvas;

    // Posisi awal item
    private Transform originalParent;
    private Vector2 originalPosition;
    private int originalSiblingIndex;
    private Vector2 originalSize;

    private bool isPlaced = false;
    private bool droppedCorrectly = false;

    private Vector3 originalScale;

    public static bool IsDragging { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        rootCanvas = GetComponentInParent<Canvas>().rootCanvas;

        // =========================================
        // SIMPAN POSISI AWAL SEJAK GAME DIMULAI
        // =========================================

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        originalSiblingIndex = transform.GetSiblingIndex();
        originalSize = rectTransform.sizeDelta;

        originalScale = transform.localScale;
    }

    // =========================================
    // HOVER LINK
    // =========================================

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPlaced && !IsDragging)
        {
            transform.localScale = originalScale * 1.05f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPlaced)
        {
            transform.localScale = originalScale;
        }
    }

    // =========================================
    // MULAI DRAG
    // =========================================

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced)
            return;

        IsDragging = true;
        droppedCorrectly = false;

        // Pindah sementara ke Canvas agar bisa
        // bergerak di atas UI lain
        transform.SetParent(rootCanvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;

        transform.localScale = originalScale * 1.05f;
    }

    // =========================================
    // SAAT DRAG
    // =========================================

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced)
            return;

        RectTransform canvasRect =
            rootCanvas.GetComponent<RectTransform>();

        Camera eventCamera =
            rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : rootCanvas.worldCamera;

        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventCamera,
            out localPosition
        );

        rectTransform.localPosition = localPosition;
    }

    // =========================================
    // SELESAI DRAG
    // =========================================

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        transform.localScale = originalScale;

        if (!droppedCorrectly && !isPlaced)
        {
            ResetPosition();
        }
    }

    // =========================================
    // MASUK DROP ZONE
    // =========================================

    public void PlaceInZone(
        Transform zone,
        Vector2 position,
        Vector2 newSize)
    {
        droppedCorrectly = true;
        isPlaced = true;

        transform.SetParent(zone, false);

        rectTransform.sizeDelta = newSize;
        rectTransform.anchoredPosition = position;

        transform.localScale = Vector3.one;

        canvasGroup.blocksRaycasts = true;
    }

    // =========================================
    // KEMBALI KE POSISI AWAL
    // =========================================

    public void ResetPosition()
    {
        isPlaced = false;
        droppedCorrectly = false;

        transform.SetParent(originalParent, false);

        rectTransform.anchoredPosition = originalPosition;
        rectTransform.sizeDelta = originalSize;

        transform.SetSiblingIndex(originalSiblingIndex);

        transform.localScale = originalScale;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}