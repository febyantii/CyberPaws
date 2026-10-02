using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    private Vector3 targetScale;

    [Header("Pengaturan Ukuran (Scale)")]
    public float hoverScale = 1.15f;  // Saat ditunjuk kursor (agak gede)
    public float pressScale = 0.90f;  // Saat dipencet (agak kecil)
    public float scaleSpeed = 15f;    // Kecepatan animasi membal

    private bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Transisi halus ke ukuran target
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    // 1. Saat kursor MASUK ke tombol -> Agak Gede
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        targetScale = originalScale * hoverScale;
    }

    // 2. Saat kursor KELUAR dari tombol -> Ukuran Normal
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        targetScale = originalScale;
    }

    // 3. Saat mouse DIPENCET (Hold/Tekan) -> Agak Kecil
    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = originalScale * pressScale;
    }

    // 4. Saat mouse DILEPAS (Release) -> Balik ke Agak Gede lagi (kalau kursor masih di atas tombol)
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHovered)
        {
            targetScale = originalScale * hoverScale;
        }
        else
        {
            targetScale = originalScale;
        }
    }
}