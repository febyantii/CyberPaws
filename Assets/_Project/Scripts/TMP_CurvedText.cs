using UnityEngine;
using TMPro;

[ExecuteAlways]
public class TMP_CurvedText : MonoBehaviour
{
    private TMP_Text textComponent;

    [Header("Pengaturan Lengkungan (Bentuk Pelangi)")]
    [Tooltip("Bentuk kurva: Kiri (0) -> Tengah Naik (1) -> Kanan Turun (0)")]
    public AnimationCurve vertexCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    );

    [Header("Tingkat Kelengkungan")]
    [Range(-50f, 50f)]
    [Tooltip("Geser slider ini untuk mengatur seberapa melengkung teksnya")]
    public float curveHeight = 8f; // Angka kecil = melengkung tipis/halus

    void OnEnable()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (textComponent == null) textComponent = GetComponent<TMP_Text>();
        if (textComponent == null) return;

        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;
        int characterCount = textInfo.characterCount;

        if (characterCount == 0) return;

        float boundsMinX = textComponent.bounds.min.x;
        float boundsMaxX = textComponent.bounds.max.x;
        float boundsWidth = boundsMaxX - boundsMinX;

        if (Mathf.Approximately(boundsWidth, 0f)) return;

        for (int i = 0; i < characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = vertices[vertexIndex + j];
                float x0 = (orig.x - boundsMinX) / boundsWidth;
                float yOffset = vertexCurve.Evaluate(x0) * curveHeight;

                vertices[vertexIndex + j] = new Vector3(orig.x, orig.y + yOffset, orig.z);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}