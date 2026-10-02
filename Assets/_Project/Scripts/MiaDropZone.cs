using UnityEngine;
using UnityEngine.EventSystems;

public class MiaDropZone : MonoBehaviour, IDropHandler
{
    public enum ZoneType
    {
        All,
        Safe,
        Scam
    }

    [Header("Zone Settings")]
    public ZoneType zoneType;

    [Header("Available Slots")]
    public Transform[] slots;

    // =========================================================
    // DROP
    // =========================================================
    public void OnDrop(PointerEventData eventData)
{
    if (eventData.pointerDrag == null)
        return;

    MiaLinkCards card =
        eventData.pointerDrag.GetComponent<MiaLinkCards>();

    if (card == null)
    {
        Debug.LogWarning("MiaLinkCards tidak ditemukan pada object yang di-drag.");
        return;
    }

    Debug.Log("DROP TERDETEKSI: " + card.gameObject.name);

    if (TryPlaceCard(card))
    {
        Debug.Log(
            card.gameObject.name +
            " berhasil masuk ke " +
            zoneType
        );
    }
}

    // =========================================================
    // TRY PLACE CARD
    // =========================================================
    public bool TryPlaceCard(MiaLinkCards card)
{
    if (card == null)
        return false;

    Transform availableSlot = GetAvailableSlot();

    if (availableSlot == null)
    {
        Debug.LogWarning(
            "Tidak ada slot kosong di " + zoneType
        );

        return false;
    }

    card.PlaceInZone(this, availableSlot);

    return true;
}
    // =========================================================
    // FIND EMPTY SLOT
    // =========================================================

    private Transform GetAvailableSlot()
    {
        if (slots == null ||
            slots.Length == 0)
        {
            return null;
        }

        foreach (Transform slot in slots)
        {
            if (slot == null)
                continue;

            // Slot kosong
            if (slot.childCount == 0)
            {
                return slot;
            }
        }

        return null;
    }

    // =========================================================
    // REMOVE CARD
    // =========================================================

    public void RemoveCard(MiaLinkCards card)
    {
        if (card == null)
            return;

        foreach (Transform slot in slots)
        {
            if (slot == null)
                continue;

            if (card.transform.parent == slot)
            {
                return;
            }
        }
    }
}
