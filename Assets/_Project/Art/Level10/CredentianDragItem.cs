
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class CredentialDragItem : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("CREDENTIAL INFORMATION")]
    public string credentialId;
    public string groupId;

    [Header("SCALE WHEN PLACED IN DROPBOX")]
    public Vector3 dropScale = new Vector3(0.4f, 1f, 1f);

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas rootCanvas;

    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private Vector3 originalScale;
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalPivot;

    private Transform previousParent;
    private Vector2 previousAnchoredPosition;
    private Vector3 previousScale;

    private CredentialDropSlot currentSlot;

    private bool isDragging;
    private bool successfullyPlaced;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
        originalAnchorMin = rectTransform.anchorMin;
        originalAnchorMax = rectTransform.anchorMax;
        originalPivot = rectTransform.pivot;

        Canvas[] canvases = GetComponentsInParent<Canvas>();

        if (canvases.Length > 0)
        {
            rootCanvas = canvases[canvases.Length - 1];
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    // =========================================
    // BEGIN DRAG
    // =========================================
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (rootCanvas == null || successfullyPlaced)
            return;

        isDragging = true;

        previousParent = transform.parent;
        previousAnchoredPosition = rectTransform.anchoredPosition;
        previousScale = rectTransform.localScale;

        // Simpan ukuran awal sebelum berpindah parent
        Vector3 currentScale = rectTransform.localScale;

        transform.SetParent(rootCanvas.transform, true);

        // Pertahankan ukuran saat drag dimulai
        rectTransform.localScale = currentScale;

        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    // =========================================
    // DRAG
    // =========================================
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        rectTransform.position = eventData.position;
    }

    // =========================================
    // END DRAG
    // =========================================
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        canvasGroup.blocksRaycasts = true;

        // Jika tidak masuk DropBox, kembalikan
        if (!successfullyPlaced)
        {
            ReturnToPreviousPosition();
        }
    }

    // =========================================
    // PLACE IN SLOT
    // =========================================
    public void PlaceInSlot(CredentialDropSlot slot)
    {
        if (slot == null)
            return;

        currentSlot = slot;
        successfullyPlaced = true;

        // Pindahkan credential ke DropBox
        transform.SetParent(slot.transform, false);

        // Atur posisi di tengah DropBox
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        // Ubah skala hanya ketika masuk DropBox
        rectTransform.localScale = dropScale;

        canvasGroup.blocksRaycasts = true;
    }

    // =========================================
    // RETURN TO PREVIOUS POSITION
    // =========================================
    public void ReturnToPreviousPosition()
    {
        if (previousParent == null)
            return;

        transform.SetParent(previousParent, false);

        rectTransform.anchorMin = originalAnchorMin;
        rectTransform.anchorMax = originalAnchorMax;
        rectTransform.pivot = originalPivot;

        rectTransform.anchoredPosition = previousAnchoredPosition;
        rectTransform.localScale = previousScale;

        currentSlot = null;
        successfullyPlaced = false;

        canvasGroup.blocksRaycasts = true;
    }

    // =========================================
    // RETURN TO ORIGINAL POSITION
    // =========================================
    public void ReturnToOrigin()
    {
        transform.SetParent(originalParent, false);

        rectTransform.anchorMin = originalAnchorMin;
        rectTransform.anchorMax = originalAnchorMax;
        rectTransform.pivot = originalPivot;

        rectTransform.anchoredPosition = originalAnchoredPosition;
        rectTransform.localScale = originalScale;

        currentSlot = null;
        successfullyPlaced = false;

        canvasGroup.blocksRaycasts = true;
    }

    // =========================================
    // REMOVE FROM SLOT
    // =========================================
    public void RemoveFromSlot()
    {
        currentSlot = null;
        successfullyPlaced = false;

        ReturnToOrigin();
    }

    // =========================================
    // GET CREDENTIAL INFORMATION
    // =========================================
    public string GetCredentialId()
    {
        return credentialId;
    }

    public string GetGroupId()
    {
        return groupId;
    }

    // =========================================
    // SET CURRENT SLOT
    // =========================================
    public void SetCurrentSlot(CredentialDropSlot slot)
    {
        currentSlot = slot;
    }

    // =========================================
    // SET DROP STATUS
    // =========================================
    public void SetDroppedSuccessfully(bool value)
    {
        successfullyPlaced = value;
    }

    // =========================================
    // GET CURRENT SLOT
    // =========================================
    public CredentialDropSlot GetCurrentSlot()
    {
        return currentSlot;
    }
}
