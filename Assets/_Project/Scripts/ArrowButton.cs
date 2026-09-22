using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArrowButton : MonoBehaviour
{
    [Header("Arrow")]
    public RectTransform arrow;

    [Header("Animation Settings")]
    public float minScale = 0.85f;
    public float maxScale = 1.15f;
    public float animationSpeed = 2f;

    [Header("Click Animation")]
    public float clickScale = 1.25f;
    public float clickDuration = 0.1f;

    private Vector3 originalScale;
    private Coroutine pulseCoroutine;

    void Start()
    {
        if (arrow == null)
        {
            arrow = GetComponent<RectTransform>();
        }

        originalScale = arrow.localScale;

        pulseCoroutine = StartCoroutine(PulseArrow());
    }

    // ========================================
    // ARROW PULSE ANIMATION
    // BESAR → KECIL → BESAR → KECIL
    // ========================================

    IEnumerator PulseArrow()
    {
        while (true)
        {
            // Kecil → Besar
            float time = 0f;

            while (time < 1f)
            {
                time += Time.deltaTime * animationSpeed;

                float scale = Mathf.Lerp(
                    minScale,
                    maxScale,
                    Mathf.SmoothStep(0f, 1f, time)
                );

                arrow.localScale = originalScale * scale;

                yield return null;
            }

            // Besar → Kecil
            time = 0f;

            while (time < 1f)
            {
                time += Time.deltaTime * animationSpeed;

                float scale = Mathf.Lerp(
                    maxScale,
                    minScale,
                    Mathf.SmoothStep(0f, 1f, time)
                );

                arrow.localScale = originalScale * scale;

                yield return null;
            }
        }
    }

    // ========================================
    // SAAT ARROW DIKLIK
    // ========================================

    public void ClickArrow()
    {
        StartCoroutine(ClickAnimation());
    }

    IEnumerator ClickAnimation()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
        }

        float time = 0f;

        // Membesar sedikit saat diklik
        while (time < 1f)
        {
            time += Time.deltaTime / clickDuration;

            float scale = Mathf.Lerp(
                1f,
                clickScale,
                time
            );

            arrow.localScale = originalScale * scale;

            yield return null;
        }

        time = 0f;

        // Kembali ke ukuran normal
        while (time < 1f)
        {
            time += Time.deltaTime / clickDuration;

            float scale = Mathf.Lerp(
                clickScale,
                1f,
                time
            );

            arrow.localScale = originalScale * scale;

            yield return null;
        }

        // Mulai pulse lagi
        pulseCoroutine = StartCoroutine(PulseArrow());
    }

    // ========================================
    // RESET
    // ========================================

    public void ResetArrow()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
        }

        arrow.localScale = originalScale;

        pulseCoroutine = StartCoroutine(PulseArrow());
    }

    // ========================================
    // STOP ANIMATION
    // ========================================

    public void StopArrowAnimation()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }

        arrow.localScale = originalScale;
    }

    // ========================================
    // RESUME ANIMATION
    // ========================================

    public void ResumeArrowAnimation()
    {
        if (pulseCoroutine == null)
        {
            pulseCoroutine = StartCoroutine(PulseArrow());
        }
    }

    // ========================================
    // CLEANUP
    // ========================================

    void OnDisable()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }
    }
}