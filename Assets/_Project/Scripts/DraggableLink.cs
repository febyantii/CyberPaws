using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private LayoutElement layoutElement;

    // Posisi awal
    private Transform originalParent;
    private Vector2 originalPosition;
    private Vector3 originalWorldPosition;
    private Quaternion originalWorldRotation;
    private int originalSiblingIndex;
    private Vector2 originalSize;
    private Vector3 originalScale;

    // Zone tempat link sedang berada
    private DropZone currentZone;

    private bool isPlaced = false;
    private bool droppedCorrectly = false;

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
        // SIMPAN UKURAN & POSISI AWAL
        // =========================================

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        // SIMPAN POSISI DUNIA SEJAK AWAL
        originalWorldPosition = rectTransform.position;
        originalWorldRotation = rectTransform.rotation;

        originalSiblingIndex = transform.GetSiblingIndex();
        originalSize = rectTransform.sizeDelta;
        originalScale = transform.localScale;

        // =========================================
        // LAYOUT ELEMENT
        // =========================================

        layoutElement = GetComponent<LayoutElement>();

        if (layoutElement == null)
        {
            layoutElement = gameObject.AddComponent<LayoutElement>();
        }

        // PENTING:
        // Ikuti ukuran ASLI link, yaitu 700 x 150
        layoutElement.minWidth = originalSize.x;
        layoutElement.preferredWidth = originalSize.x;

        layoutElement.minHeight = originalSize.y;
        layoutElement.preferredHeight = originalSize.y;

        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;
    }

    // =========================================
    // HOVER
    // =========================================

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsDragging)
        {
            transform.localScale = originalScale * 1.05f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsDragging)
        {
            transform.localScale = originalScale;
        }
    }

    // =========================================
    // MULAI DRAG
    // =========================================

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        droppedCorrectly = false;

        // Kalau sebelumnya ada di Server / Trash
        if (currentZone != null)
        {
            currentZone.RemovePlacedItem(this);
            DropZone.NotifyLinkRemoved(this);

            currentZone = null;
        }

        // Pindahkan sementara ke Canvas utama
        transform.SetParent(rootCanvas.transform, true);

        // Tampilkan paling depan saat sedang di-drag
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

        // Kalau drop tidak berhasil,
        // kembali ke tempat awal
        if (!droppedCorrectly)
        {
            ResetPosition();
        }
    }

    // =========================================
    // MASUK ZONE
    // =========================================

    public void PlaceInZone(
        DropZone zone,
        Transform contentParent)
    {
        droppedCorrectly = true;
        isPlaced = true;

        currentZone = zone;

        // Masuk ke ServerItems / TrashItems
        transform.SetParent(contentParent, false);

        // PENTING:
        // JANGAN ubah sizeDelta menjadi ukuran kecil
        rectTransform.sizeDelta = originalSize;

        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;

        // Pastikan layout tetap memakai ukuran asli
        layoutElement.minWidth = originalSize.x;
        layoutElement.preferredWidth = originalSize.x;
        layoutElement.minHeight = originalSize.y;
        layoutElement.preferredHeight = originalSize.y;

        canvasGroup.blocksRaycasts = true;
    }

    // =========================================
    // BALIK KE POSISI AWAL
    // =========================================

    public void ResetPosition()
    {
        isPlaced = false;
        droppedCorrectly = false;

        // Hapus dari zone
        if (currentZone != null)
        {
            currentZone.RemovePlacedItem(this);
        }

        DropZone.NotifyLinkRemoved(this);

        currentZone = null;

        // =========================================
        // KEMBALI KE PARENT AWAL
        // =========================================

        transform.SetParent(originalParent, false);

        // Kembalikan ukuran asli
        rectTransform.sizeDelta = originalSize;

        // Kembalikan posisi berdasarkan WORLD POSITION
        rectTransform.position = originalWorldPosition;

        // Kembalikan rotasi
        rectTransform.rotation = originalWorldRotation;

        // Kembalikan scale
        transform.localScale = originalScale;

        // Kembalikan urutan hierarchy
        transform.SetSiblingIndex(
            Mathf.Clamp(
                originalSiblingIndex,
                0,
                originalParent.childCount - 1
            )
        );

        // Pastikan terlihat
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}