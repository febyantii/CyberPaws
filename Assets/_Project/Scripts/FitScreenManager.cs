using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitScreenManager : MonoBehaviour
{
    [Header("Ukuran Peta")]
    public float mapWidth = 26.6f;
    public float mapHeight = 10.0f;

    [Header("Titik Tengah Peta")]
    public Vector2 mapCenter = new Vector2(-0.8f, -0.32f);

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        ApplyCameraFit();
    }

    void Update()
    {
        ApplyCameraFit();
    }

    void ApplyCameraFit()
    {
        if (cam == null || Screen.height <= 0) return;

        transform.position = new Vector3(mapCenter.x, mapCenter.y, -10f);

        float screenAspect = (float)Screen.width / (float)Screen.height;

        float sizeBasedOnHeight = mapHeight / 2f;
        float sizeBasedOnWidth = (mapWidth / 2f) / screenAspect;

        cam.orthographicSize = Mathf.Max(sizeBasedOnHeight, sizeBasedOnWidth);
    }
}