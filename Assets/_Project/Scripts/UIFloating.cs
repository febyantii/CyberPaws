using UnityEngine;

public class UIFloating : MonoBehaviour
{
    [Header("Floating Settings")]
    public float speed = 2f;       // Kecepatan naik-turun
    public float amplitude = 10f;   // Jarak/tinggi melayang (pixel)
    
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}