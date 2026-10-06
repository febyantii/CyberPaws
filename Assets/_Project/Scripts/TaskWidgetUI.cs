using UnityEngine;
using UnityEngine.UI;

public class TaskWidgetUI : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject taskBar;
    [SerializeField] private GameObject taskPopupPanel;

    [Header("Buttons")]
    [SerializeField] private Button taskBarButton;
    [SerializeField] private Button closeButton;

    [Header("Bouncy Animation Settings")]
    [SerializeField] private float bounceSpeed = 4f;   // Kecepatan denyut/bouncy
    [SerializeField] private float bounceAmount = 0.08f; // Besarnya perubahan skala (8%)

    private Vector3 originalScale;

    private void Awake()
    {
        if (taskBar != null)
        {
            originalScale = taskBar.transform.localScale;
        }
    }

    private void Start()
    {
        // Pasang fungsi ke tombol saat diklik
        if (taskBarButton != null)
            taskBarButton.onClick.AddListener(OpenPopup);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePopup);

        // Kondisi awal: TaskBar aktif, Popup tertutup
        ClosePopup();
    }

    private void Update()
    {
        // Efek Bouncy (Membesar-Mengecil) secara terus-menerus saat TaskBar aktif
        if (taskBar != null && taskBar.activeSelf)
        {
            float scaleOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceAmount;
            taskBar.transform.localScale = originalScale + new Vector3(scaleOffset, scaleOffset, 0f);
        }
    }

    public void OpenPopup()
    {
        if (taskBar != null) taskBar.SetActive(false);
        if (taskPopupPanel != null) taskPopupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        if (taskPopupPanel != null) taskPopupPanel.SetActive(false);
        
        if (taskBar != null)
        {
            taskBar.SetActive(true);
            taskBar.transform.localScale = originalScale; // Reset skala ke awal
        }
    }
}