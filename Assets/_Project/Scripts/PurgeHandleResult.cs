using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PurgeHandleResult : MonoBehaviour, IPointerUpHandler
{
    [Header("References")]
    public Slider slider;

    [Header("Controller")]
    public AntivirusPurgeController controller;

    [Header("Safe Zone")]
    [Range(0f, 1f)]
    public float safeZoneStart = 0.80f;

    public void OnPointerUp(PointerEventData eventData)
    {
        CheckResult();
    }

    void CheckResult()
    {
        if (slider == null || controller == null)
            return;

        // Jika handle sudah masuk area kanan / safe zone
        if (slider.value >= safeZoneStart)
        {
            controller.CheckResult(true);
        }
        else
        {
            controller.CheckResult(false);
        }
    }
}