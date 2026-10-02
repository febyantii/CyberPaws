using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PurgeHandleResult : MonoBehaviour, IEndDragHandler
{
    [Header("References")]
    public Slider slider;

    [Header("Controller")]
    public AntivirusPurgeController controller;

    [Header("Safe Zone")]
    [Range(0f, 1f)]
    public float safeZoneStart = 0.80f;

    void Start()
    {
        // Kalau Slider belum diisi dari Inspector,
        // ambil Slider dari object ini.
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    // Dipanggil setelah drag Slider selesai
    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(CheckAfterDrag());
    }

    IEnumerator CheckAfterDrag()
    {
        // Tunggu 1 frame supaya nilai Slider
        // sudah benar-benar diperbarui.
        yield return null;

        CheckResult();
    }

    void CheckResult()
    {
        if (slider == null)
        {
            Debug.LogError("PurgeHandleResult: Slider belum diisi!");
            return;
        }

        if (controller == null)
        {
            Debug.LogError("PurgeHandleResult: Controller belum diisi!");
            return;
        }

        Debug.Log("Slider Value = " + slider.value);

        if (slider.value >= safeZoneStart)
        {
            Debug.Log("CORRECT - masuk Safe Zone");
            controller.CheckResult(true);
        }
        else
        {
            Debug.Log("TRY AGAIN - belum masuk Safe Zone");
            controller.CheckResult(false);
        }
    }
}