using System;
using UnityEngine;
using UnityEngine.UI;

public class MinigamePopup : MonoBehaviour
{
    [Header("UI Components")]
    public Button closeButton; // Drag tombol close/X di Inspector

    private Action onCloseCallback;

    public void SetupMinigame(Action onClose)
    {
        onCloseCallback = onClose;

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(ClosePopup);
        }
    }

    public void ClosePopup()
    {
        // Jalankan callback buat balikin kondisi game
        onCloseCallback?.Invoke();

        // Hapus popup minigame dari layar
        Destroy(gameObject);
    }
}