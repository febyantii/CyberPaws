using UnityEngine;
using TMPro;

public class SecurityScoreDisplay : MonoBehaviour
{
    [Header("UI Text Component")]
    public TextMeshProUGUI scoreText; // Drag TextMeshPro angka skor di sini

    private void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        int currentScore = CityScoreManager.GetScore();

        if (scoreText != null)
        {
            scoreText.text = currentScore + "%";
        }
    }
}