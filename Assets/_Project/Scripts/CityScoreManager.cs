using UnityEngine;

public static class CityScoreManager
{
    private const string SCORE_KEY = "CitySecurityScore";
    private const int DEFAULT_SCORE = 90; // Skor awal game (90%)

    public static int GetScore()
    {
        return PlayerPrefs.GetInt(SCORE_KEY, DEFAULT_SCORE);
    }

    public static void ModifyScore(int amount)
    {
        int currentScore = GetScore();
        currentScore += amount;

        // Batasi nilai skor agar selalu di rentang 0% - 100%
        currentScore = Mathf.Clamp(currentScore, 0, 100);

        PlayerPrefs.SetInt(SCORE_KEY, currentScore);
        PlayerPrefs.Save();
    }

    // --- FUNGSI BARU ---
    // Panggil ini saat tombol "Play" atau "New Game" di Main Menu diklik
    public static void ResetScore()
    {
        PlayerPrefs.SetInt(SCORE_KEY, DEFAULT_SCORE);
        PlayerPrefs.Save();
    }
}