using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FrankieGameDetailManager : MonoBehaviour
{
    [Header("========== PANELS ==========")]

    [SerializeField] private GameObject securityPanel;
    [SerializeField] private GameObject seePatternPanel;
    [SerializeField] private GameObject answerPanel;


    [Header("========== SEE PATTERN UI ==========")]

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text viewTimesText;


    [Header("========== ANSWER UI ==========")]

    [SerializeField] private TMP_Text answerViewTimesText;
    [SerializeField] private Button answerSeePatternButton;


    [Header("========== PROGRESS BARS ==========")]

    [SerializeField] private FrankieProgressBar progressBar1;
    [SerializeField] private FrankieProgressBar progressBar2;
    [SerializeField] private FrankieProgressBar progressBar3;


    [Header("========== CORRECT PATTERN ==========")]

    [SerializeField] private int correctSlot1 = 2;
    [SerializeField] private int correctSlot2 = 6;
    [SerializeField] private int correctSlot3 = 4;


    [Header("========== SETTINGS ==========")]

    [SerializeField] private float patternViewDuration = 3f;
    [SerializeField] private int maxPatternViews = 3;


    [Header("========== RESULT ==========")]

    [SerializeField] private FrankieGameManager frankieGameManager;


    private int patternViewsUsed = 0;
    private bool isViewingPattern = false;
    private Coroutine patternCoroutine;


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        SetupGameDetail();
    }


    // =====================================================
    // INITIAL SETUP
    // =====================================================

    private void SetupGameDetail()
    {
        securityPanel.SetActive(true);
        seePatternPanel.SetActive(false);
        answerPanel.SetActive(false);

        patternViewsUsed = 0;

        UpdateViewCounter();
    }


    // =====================================================
    // SECURITY → SEE PATTERN
    // =====================================================

    public void StartPatternView()
    {
        if (isViewingPattern)
            return;

        if (patternViewsUsed >= maxPatternViews)
        {
            Debug.Log("FRANKIE: Semua kesempatan melihat pattern sudah habis.");
            return;
        }

        if (patternCoroutine != null)
            StopCoroutine(patternCoroutine);

        patternCoroutine = StartCoroutine(ViewPatternCoroutine());
    }


    // =====================================================
    // PATTERN COUNTDOWN
    // =====================================================

    private IEnumerator ViewPatternCoroutine()
    {
        isViewingPattern = true;

        patternViewsUsed++;

        securityPanel.SetActive(false);
        answerPanel.SetActive(false);
        seePatternPanel.SetActive(true);

        UpdateViewCounter();

        float remainingTime = patternViewDuration;

        while (remainingTime > 0f)
        {
            timerText.text =
                Mathf.CeilToInt(remainingTime).ToString();

            yield return null;

            remainingTime -= Time.deltaTime;
        }

        timerText.text = "0";

        yield return new WaitForSeconds(0.2f);

        isViewingPattern = false;

        OpenAnswerPanel();
    }


    // =====================================================
    // OPEN ANSWER PANEL
    // =====================================================

    private void OpenAnswerPanel()
    {
        seePatternPanel.SetActive(false);
        answerPanel.SetActive(true);

        UpdateViewCounter();

        UpdateSeePatternButton();

        Debug.Log("FRANKIE: Masuk Answer Panel.");
    }


    // =====================================================
    // ANSWER → SEE PATTERN AGAIN
    // =====================================================

    public void SeePatternAgain()
    {
        if (isViewingPattern)
            return;

        if (patternViewsUsed >= maxPatternViews)
        {
            Debug.Log(
                "FRANKIE: Tidak ada kesempatan melihat pattern lagi."
            );

            return;
        }

        StartPatternView();
    }


    // =====================================================
    // UPDATE VIEW COUNTER
    // =====================================================

    private void UpdateViewCounter()
    {
        string counter =
            patternViewsUsed + " / " + maxPatternViews;

        if (viewTimesText != null)
            viewTimesText.text = counter;

        if (answerViewTimesText != null)
            answerViewTimesText.text = counter;
    }


    // =====================================================
    // UPDATE SEE PATTERN BUTTON
    // =====================================================

    private void UpdateSeePatternButton()
    {
        if (answerSeePatternButton == null)
            return;

        answerSeePatternButton.interactable =
            patternViewsUsed < maxPatternViews;
    }


    // =====================================================
    // CHECK ANSWER
    // =====================================================

    public void CheckAnswer()
    {
        if (progressBar1 == null ||
            progressBar2 == null ||
            progressBar3 == null)
        {
            Debug.LogError(
                "FRANKIE: ProgressBar belum diisi!"
            );

            return;
        }


        int answer1 =
            progressBar1.GetNearestSlot();

        int answer2 =
            progressBar2.GetNearestSlot();

        int answer3 =
            progressBar3.GetNearestSlot();


        Debug.Log(
            "FRANKIE ANSWER: " +
            "Bar1 = Slot " + answer1 +
            " | Bar2 = Slot " + answer2 +
            " | Bar3 = Slot " + answer3
        );


        bool correct =
            answer1 == correctSlot1 &&
            answer2 == correctSlot2 &&
            answer3 == correctSlot3;


        if (correct)
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }


    // =====================================================
    // CORRECT
    // =====================================================

    private void CorrectAnswer()
    {
        Debug.Log("FRANKIE: SEMUA JAWABAN BENAR!");

        if (frankieGameManager != null)
            frankieGameManager.ShowCongratulation();
    }


    // =====================================================
    // WRONG
    // =====================================================

    private void WrongAnswer()
    {
        Debug.Log("FRANKIE: JAWABAN SALAH!");

        if (frankieGameManager != null)
            frankieGameManager.ShowTryAgain();
    }
}