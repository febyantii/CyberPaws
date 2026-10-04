using UnityEngine;
using UnityEngine.EventSystems;

public class FrankieProgressBar : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("========== SLOTS 0 - 8 ==========")]
    [SerializeField] private RectTransform[] slots = new RectTransform[9];

    private RectTransform buttonBar;
    private RectTransform progressBar;

    private Canvas canvas;
    private Camera uiCamera;

    private Vector2 dragOffset;

    // =====================================================
    // AWAKE
    // =====================================================

    private void Awake()
    {
        // ButtonBar yang ditempeli script
        buttonBar = GetComponent<RectTransform>();

        // Parent langsung dari ButtonBar
        // = ProgressBar1 / ProgressBar2 / ProgressBar3
        progressBar = buttonBar.parent as RectTransform;

        // Cari Canvas
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null &&
            canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }
    }

    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        Canvas.ForceUpdateCanvases();

        if (!CheckSlots())
            return;

        // Button mulai di Slot0
        MoveButtonToSlot(0);
    }

    // =====================================================
    // CEK SLOT
    // =====================================================

    private bool CheckSlots()
    {
        if (slots == null)
        {
            Debug.LogError(
                "FRANKIE: Array Slots belum diisi!"
            );

            return false;
        }

        if (slots.Length < 9)
        {
            Debug.LogError(
                "FRANKIE: Harus ada Slot0 sampai Slot8!"
            );

            return false;
        }

        for (int i = 0; i < 9; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogError(
                    "FRANKIE: Slot " + i +
                    " belum diisi di Inspector!"
                );

                return false;
            }
        }

        if (progressBar == null)
        {
            Debug.LogError(
                "FRANKIE: ButtonBar harus berada langsung di dalam ProgressBar!"
            );

            return false;
        }

        return true;
    }

    // =====================================================
    // POINTER DOWN
    // =====================================================

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CheckSlots())
            return;

        Vector2 mousePosition;

        bool success =
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                progressBar,
                eventData.position,
                uiCamera,
                out mousePosition
            );

        if (!success)
            return;

        // Supaya Button tidak langsung lompat ketika mulai drag
        dragOffset =
            buttonBar.localPosition -
            new Vector3(
                mousePosition.x,
                buttonBar.localPosition.y,
                buttonBar.localPosition.z
            );

        Debug.Log("FRANKIE: BUTTON DOWN");
    }

    // =====================================================
    // DRAG
    // =====================================================

    public void OnDrag(PointerEventData eventData)
    {
        if (!CheckSlots())
            return;

        Vector2 mousePosition;

        bool success =
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                progressBar,
                eventData.position,
                uiCamera,
                out mousePosition
            );

        if (!success)
            return;

        // Posisi mouse dalam coordinate ProgressBar
        float newX =
            mousePosition.x +
            dragOffset.x;

        // =================================================
        // AMBIL POSISI SLOT0 DAN SLOT8
        // =================================================

        float slot0X =
            GetSlotLocalPosition(0).x;

        float slot8X =
            GetSlotLocalPosition(8).x;

        // Pastikan batas kiri/kanan benar
        float minX =
            Mathf.Min(slot0X, slot8X);

        float maxX =
            Mathf.Max(slot0X, slot8X);

        // Button tidak boleh keluar dari Slot0 - Slot8
        newX =
            Mathf.Clamp(
                newX,
                minX,
                maxX
            );

        // =================================================
        // GERAKKAN BUTTON
        // =================================================

        buttonBar.localPosition =
            new Vector3(
                newX,
                buttonBar.localPosition.y,
                buttonBar.localPosition.z
            );
    }

    // =====================================================
    // POINTER UP
    // =====================================================

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CheckSlots())
            return;

        // Cari slot yang paling dekat
        int nearestSlot =
            GetNearestSlot();

        // Pindahkan Button tepat ke slot tersebut
        MoveButtonToSlot(nearestSlot);

        Debug.Log(
            "FRANKIE: BUTTON BERADA DI SLOT " +
            nearestSlot
        );
    }

    // =====================================================
    // GET SLOT POSITION
    // =====================================================

    private Vector3 GetSlotLocalPosition(int slotIndex)
    {
        // Ambil posisi Slot dalam WORLD
        Vector3 worldPosition =
            slots[slotIndex].position;

        // Ubah WORLD POSITION
        // menjadi POSITION relatif terhadap ProgressBar
        Vector3 localPosition =
            progressBar.InverseTransformPoint(
                worldPosition
            );

        return localPosition;
    }

    // =====================================================
    // CARI SLOT TERDEKAT
    // =====================================================

    public int GetNearestSlot()
    {
        if (!CheckSlots())
            return 0;

        float buttonX =
            buttonBar.localPosition.x;

        int nearestSlot = 0;

        float smallestDistance =
            Mathf.Abs(
                buttonX -
                GetSlotLocalPosition(0).x
            );

        // Cek Slot1 sampai Slot8
        for (int i = 1; i < 9; i++)
        {
            float slotX =
                GetSlotLocalPosition(i).x;

            float distance =
                Mathf.Abs(
                    buttonX -
                    slotX
                );

            if (distance < smallestDistance)
            {
                smallestDistance =
                    distance;

                nearestSlot =
                    i;
            }
        }

        return nearestSlot;
    }

    // =====================================================
    // PINDAHKAN BUTTON KE SLOT
    // =====================================================

    private void MoveButtonToSlot(int slotIndex)
    {
        if (!CheckSlots())
            return;

        slotIndex =
            Mathf.Clamp(
                slotIndex,
                0,
                8
            );

        Vector3 slotPosition =
            GetSlotLocalPosition(
                slotIndex
            );

        // Hanya X yang mengikuti slot.
        // Y Button tetap.
        // Ukuran dan bentuk Button tetap.
        buttonBar.localPosition =
            new Vector3(
                slotPosition.x,
                buttonBar.localPosition.y,
                buttonBar.localPosition.z
            );
    }

    // =====================================================
    // CEK BUTTON ADA DI SLOT TERTENTU
    // =====================================================

    public bool IsAtSlot(int slotIndex)
    {
        return GetNearestSlot() == slotIndex;
    }
}