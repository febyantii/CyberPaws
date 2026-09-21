using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowPulseAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float maxScale = 1.15f;
    public float pulseSpeed = 4f;

    private Vector3 originalScale;
    private bool isHovering = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isHovering)
        {
            float pulse = 1f + (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f * (maxScale - 1f);

            transform.localScale = originalScale * pulse;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}