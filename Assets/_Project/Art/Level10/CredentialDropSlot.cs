
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CredentialDropSlot : MonoBehaviour, IDropHandler
{
    [Header("DROP SLOT INFORMATION")]
    public string groupId;
    public string correctCredentialId;

    private CredentialDragItem currentItem;

    public event Action OnSlotChanged;

    // =================================
    // DROP EVENT
    // =================================

    public void OnDrop(PointerEventData eventData)
    {
        CredentialDragItem draggedItem =
            eventData.pointerDrag != null
            ? eventData.pointerDrag.GetComponent<CredentialDragItem>()
            : null;

        if (draggedItem == null)
            return;

        // Check matching group
        if (draggedItem.GetGroupId() != groupId)
            return;

        // Replace previous item if necessary
        if (currentItem != null && currentItem != draggedItem)
        {
            currentItem.ReturnToOrigin();
        }

        currentItem = draggedItem;

        draggedItem.PlaceInSlot(this);

        OnSlotChanged?.Invoke();
    }

    // =================================
    // CHECK SLOT
    // =================================

    public bool IsFilled()
    {
        return currentItem != null;
    }

    public bool IsCorrect()
    {
        if (currentItem == null)
            return false;

        return currentItem.GetCredentialId() ==
               correctCredentialId;
    }

    // =================================
    // REMOVE ITEM
    // =================================

    public void RemoveItem(CredentialDragItem item)
    {
        if (currentItem == item)
        {
            currentItem = null;

            if (item != null)
            {
                item.SetCurrentSlot(null);
            }

            OnSlotChanged?.Invoke();
        }
    }

    // =================================
    // CLEAR SLOT
    // =================================

    public void ClearSlot()
    {
        if (currentItem != null)
        {
            currentItem.SetCurrentSlot(null);
        }

        currentItem = null;

        OnSlotChanged?.Invoke();
    }
}
