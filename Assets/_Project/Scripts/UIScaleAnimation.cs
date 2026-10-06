using System.Collections;
using UnityEngine;

public class UIScaleAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Durasi animasi dalam detik")]
    public float duration = 0.5f;

    [Tooltip("Kurva gerakan scale (Bisa diatur grafiknya di Inspector)")]
    public AnimationCurve scaleCurve = new AnimationCurve(
        new Keyframe(0f, 0f),       // Mulai dari ukuran 0
        new Keyframe(0.5f, 1.15f),  // Membesar melebihi ukuran normal (Pop)
        new Keyframe(0.75f, 0.95f), // Mengecil sedikit
        new Keyframe(1f, 1f)        // Kembali ke ukuran normal 100%
    );

    private Vector3 originalScale;

    void Awake()
    {
        // Simpan ukuran asli objek saat pertama kali
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        // Setiap kali objek/popup di-aktifkan (SetActive(true)), animasi jalan otomatis
        StopAllCoroutines();
        StartCoroutine(AnimateScale());
    }

    IEnumerator AnimateScale()
    {
        float timer = 0f;
        transform.localScale = Vector3.zero; // Mulai dari kecil/nol

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            // Mengambil nilai skala berdasarkan kurva
            float curveValue = scaleCurve.Evaluate(progress);
            transform.localScale = originalScale * curveValue;

            yield return null; // Tunggu ke frame berikutnya
        }

        transform.localScale = originalScale; // Pastikan posisi akhir tepat
    }
}