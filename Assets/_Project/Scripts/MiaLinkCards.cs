using UnityEngine;
using UnityEngine.EventSystems;

public class MiaLinkCards : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("Answer")]
    public bool isSafe;

    [Header("References")]
    public Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // =========================================================
    // HOME / ALL BOX
    // =========================================================

    private Transform homeParent;
    private Vector2 homePosition;
    private Vector3 homeScale;
    private Quaternion homeRotation;

    // =========================================================
    // PREVIOUS LOCATION
    // =========================================================

    private Transform previousParent;
    private Vector2 previousPosition;
    private Vector3 previousScale;
    private Quaternion previousRotation;
    private MiaDropZone previousZone;

    // =========================================================
    // CURRENT STATE
    // =========================================================

    private MiaDropZone currentZone;

    private bool wasDropped;
    private bool isDragging;

    // =========================================================
    // ORIGINAL CARD SIZE
    // =========================================================

    private Vector2 originalSize;

    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        // Simpan ukuran asli card
        originalSize = rectTransform.rect.size;
    }

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        SaveHomePosition();
    }

    // =========================================================
    // SAVE HOME
    // =========================================================

    private void SaveHomePosition()
    {
        homeParent = transform.parent;

        homePosition = rectTransform.anchoredPosition;
        homeScale = rectTransform.localScale;
        homeRotation = rectTransform.localRotation;
    }

    // =========================================================
    // BEGIN DRAG
    // =========================================================

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
        {
            Debug.LogWarning(
                "Canvas belum di-assign pada " +
                gameObject.name
            );

            return;
        }

        isDragging = true;
        wasDropped = false;

        // Simpan posisi sebelum drag
        previousParent = transform.parent;
        previousPosition = rectTransform.anchoredPosition;
        previousScale = rectTransform.localScale;
        previousRotation = rectTransform.localRotation;
        previousZone = currentZone;

        canvasGroup.alpha = 0.75f;

        // Supaya DropZone bisa menerima raycast
        canvasGroup.blocksRaycasts = false;

        // Pindah sementara ke Canvas
        transform.SetParent(canvas.transform, true);

        transform.SetAsLastSibling();
    }

    // =========================================================
    // DRAG
    // =========================================================

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || canvas == null)
            return;

        RectTransform canvasRect =
            canvas.transform as RectTransform;

        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }

    // =========================================================
    // END DRAG
    // =========================================================

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Kalau tidak masuk DropZone
        if (!wasDropped)
        {
            ReturnToPreviousLocation();
        }
    }

    // =========================================================
    // DROP ZONE
    // =========================================================

    public void SetDropZone(MiaDropZone zone)
    {
        if (zone == null)
            return;

        if (zone.TryPlaceCard(this))
        {
            wasDropped = true;
            currentZone = zone;
        }
    }

    // =========================================================
    // PLACE IN ZONE
    // =========================================================

    public void PlaceInZone(
        MiaDropZone zone,
        Transform slot)
    {
        if (zone == null || slot == null)
            return;

        currentZone = zone;

        MoveToSlot(slot);

        wasDropped = true;
    }

    // =========================================================
    // MOVE TO SLOT
    // =========================================================

    public void MoveToSlot(Transform slot)
    {
        if (slot == null)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Slot tidak ditemukan."
            );

            return;
        }

        // =====================================================
        // PINDAHKAN KE SLOT
        // =====================================================

        transform.SetParent(slot, false);

        // =====================================================
        // JANGAN STRETCH MENGIKUTI SLOT
        // =====================================================

        rectTransform.anchorMin =
            new Vector2(0.5f, 0.5f);

        rectTransform.anchorMax =
            new Vector2(0.5f, 0.5f);

        rectTransform.pivot =
            new Vector2(0.5f, 0.5f);

        // =====================================================
        // POSISI TENGAH SLOT
        // =====================================================

        rectTransform.anchoredPosition =
            Vector2.zero;

        // =====================================================
        // PERTAHANKAN UKURAN ASLI
        // =====================================================

        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            originalSize.x
        );

        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            originalSize.y
        );

        // =====================================================
        // ROTATION
        // =====================================================

        rectTransform.localRotation =
            Quaternion.identity;

        // =====================================================
        // SCALE
        // =====================================================

        rectTransform.localScale =
            Vector3.one;
    }

    // =========================================================
    // CHECK CORRECT ANSWER
    // =========================================================

    public bool IsCorrect()
    {
        if (currentZone == null)
            return false;

        if (currentZone.zoneType ==
            MiaDropZone.ZoneType.All)
        {
            return false;
        }

        bool placedInSafe =
            currentZone.zoneType ==
            MiaDropZone.ZoneType.Safe;

        return placedInSafe == isSafe;
    }

    // =========================================================
    // RETURN TO PREVIOUS LOCATION
    // =========================================================

    public void ReturnToPreviousLocation()
    {
        if (previousParent == null)
            return;

        transform.SetParent(
            previousParent,
            false
        );

        rectTransform.anchoredPosition =
            previousPosition;

        rectTransform.localScale =
            previousScale;

        rectTransform.localRotation =
            previousRotation;

        currentZone =
            previousZone;

        wasDropped = false;
    }

    // =========================================================
    // RETURN TO ORIGINAL ALL SLOT
    // =========================================================

    public void ReturnToOriginalAllSlot()
    {
        ResetToHome();
    }

    // =========================================================
    // RESET TO HOME / ALL BOX
    // =========================================================

    public void ResetToHome()
    {
        if (homeParent == null)
            return;

        transform.SetParent(
            homeParent,
            false
        );

        rectTransform.anchoredPosition =
            homePosition;

        rectTransform.localScale =
            homeScale;

        rectTransform.localRotation =
            homeRotation;

        currentZone = null;

        wasDropped = false;
    }

    // =========================================================
    // GET CURRENT ZONE
    // =========================================================

    public MiaDropZone GetCurrentZone()
    {
        return currentZone;
    }

    // =========================================================
    // CHECK ALL BOX
    // =========================================================

    public bool IsInAllBox()
    {
        return currentZone == null;
    }
}
