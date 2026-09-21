using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Karakter")]
    public Transform target;

    [Header("Pengaturan Zoom")]
    public float zoomSize = 2.2f; 
    public float smoothSpeed = 8f;

    [Header("Batas Peta (Sesuai Collider MapBoundaries)")]
    public float minX = -13.5f;
    public float maxX = 13.5f;
    public float minY = -5.0f;
    public float maxY = 5f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        if (target == null)
        {
            GameObject pip = GameObject.Find("AgentPip");
            if (pip != null)
            {
                target = pip.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        cam.orthographicSize = zoomSize;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float clampedX = Mathf.Clamp(target.position.x, minX + camWidth, maxX - camWidth);
        float clampedY = Mathf.Clamp(target.position.y, minY + camHeight, maxY - camHeight);

        Vector3 targetPosition = new Vector3(clampedX, clampedY, -10f);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}