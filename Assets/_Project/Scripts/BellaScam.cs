using UnityEngine;

public class BellaScam : MonoBehaviour
{
    [Header("PAGES - TENTUKAN SENDIRI")]
    public GameObject pageBackground;
    public GameObject pageSMS;
    public GameObject pageMessage1;
    public GameObject pageMessage2;
    public GameObject pageMessage3;
    public GameObject pageCorrect;
    public GameObject pageWrong;
    public GameObject pageSuccess;

    private int currentMessage = 0;

    void Start()
    {
        ShowPage(pageBackground);
    }

    public void OpenSMS()
    {
        ShowPage(pageSMS);
    }

    public void OpenMessage1()
    {
        currentMessage = 1;
        ShowPage(pageMessage1);
    }

    public void OpenMessage2()
    {
        currentMessage = 2;
        ShowPage(pageMessage2);
    }

    public void OpenMessage3()
    {
        currentMessage = 3;
        ShowPage(pageMessage3);
    }

    public void Message1Scam()
    {
        ShowPage(pageCorrect);
    }

    public void Message1Safe()
    {
        ShowPage(pageWrong);
    }

    public void Message2Scam()
    {
        ShowPage(pageCorrect);
    }

    public void Message2Safe()
    {
        ShowPage(pageWrong);
    }

    public void Message3Scam()
    {
        ShowPage(pageWrong);
    }

    public void Message3Safe()
    {
        ShowPage(pageCorrect);
    }

    public void ContinueAfterCorrect()
    {
        if (currentMessage == 1 || currentMessage == 2)
        {
            ShowPage(pageSMS);
        }
        else if (currentMessage == 3)
        {
            ShowPage(pageSuccess);
        }
    }

    public void TryAgain()
    {
        if (currentMessage == 1)
        {
            ShowPage(pageMessage1);
        }
        else if (currentMessage == 2)
        {
            ShowPage(pageMessage2);
        }
        else if (currentMessage == 3)
        {
            ShowPage(pageMessage3);
        }
    }

    public void ShowPage(GameObject selectedPage)
    {
        if (pageBackground != null)
            pageBackground.SetActive(false);

        if (pageSMS != null)
            pageSMS.SetActive(false);

        if (pageMessage1 != null)
            pageMessage1.SetActive(false);

        if (pageMessage2 != null)
            pageMessage2.SetActive(false);

        if (pageMessage3 != null)
            pageMessage3.SetActive(false);

        if (pageCorrect != null)
            pageCorrect.SetActive(false);

        if (pageWrong != null)
            pageWrong.SetActive(false);

        if (pageSuccess != null)
            pageSuccess.SetActive(false);

        if (selectedPage != null)
            selectedPage.SetActive(true);
    }
}