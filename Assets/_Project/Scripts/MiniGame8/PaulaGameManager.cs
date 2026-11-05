
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaulaGameManager : MonoBehaviour
{
    [System.Serializable]
    public class VoucherSlot
    {
        [Header("Voucher Button")]
        public Button voucherButton;

        [Header("Actual Voucher Image")]
        public GameObject voucherImage;

        [Header("Background Sprites")]
        public Sprite normalSprite;
        public Sprite selectedSprite;

        [Header("Correct Answer")]
        public bool isCorrect;

        [HideInInspector]
        public Image backgroundImage;
    }

    [Header("PAULA PANELS")]
    public GameObject paulaProfilePanel;
    public GameObject paulaMessagePanel;
    public GameObject paulaVoucherPanel;
    public GameObject paulaCongratulationPanel;
    public GameObject paulaFailedPanel;

    [Header("NAVIGATION BUTTONS")]
    public Button seeMessageButton;
    public Button checkVoucherButton;
    public Button backToMainButton;
    public Button tryAgainButton;

    [Header("VOUCHER SLOTS")]
    public VoucherSlot[] vouchers = new VoucherSlot[4];

    [Header("SELECTION SETTINGS")]
    public TMP_Text selectedText;
    public Button submitButton;

    [Header("MAIN PANEL MANAGER")]
    public MainPanelManager8 mainPanelManager;

    private List<int> selectedVouchers = new List<int>();

    private const int MAX_SELECTION = 2;

    private void Start()
    {
        SetupNavigationButtons();
        SetupVoucherButtons();

        ResetVoucherSelection();
        ShowProfilePanel();
    }

    // =====================================
    // BUTTON SETUP
    // =====================================

    private void SetupNavigationButtons()
    {
        seeMessageButton.onClick.AddListener(ShowMessagePanel);
        checkVoucherButton.onClick.AddListener(ShowVoucherPanel);
        backToMainButton.onClick.AddListener(BackToMainPanel);
        tryAgainButton.onClick.AddListener(ResetVoucherPanel);
    }

    private void SetupVoucherButtons()
    {
        for (int i = 0; i < vouchers.Length; i++)
        {
            int index = i;

            vouchers[index].backgroundImage =
                vouchers[index].voucherButton.GetComponent<Image>();

            vouchers[index].voucherButton.onClick.AddListener(
                () => SelectVoucher(index)
            );
        }

        submitButton.onClick.AddListener(SubmitVouchers);
    }

    // =====================================
    // PANEL NAVIGATION
    // =====================================

    private void HideAllPanels()
    {
        paulaProfilePanel.SetActive(false);
        paulaMessagePanel.SetActive(false);
        paulaVoucherPanel.SetActive(false);
        paulaCongratulationPanel.SetActive(false);
        paulaFailedPanel.SetActive(false);
    }

    public void ShowProfilePanel()
    {
        HideAllPanels();
        paulaProfilePanel.SetActive(true);
    }

    public void ShowMessagePanel()
    {
        HideAllPanels();
        paulaMessagePanel.SetActive(true);
    }

    public void ShowVoucherPanel()
    {
        HideAllPanels();
        paulaVoucherPanel.SetActive(true);
    }

    public void ShowCongratulationPanel()
    {
        HideAllPanels();
        paulaCongratulationPanel.SetActive(true);
    }

    public void ShowFailedPanel()
    {
        HideAllPanels();
        paulaFailedPanel.SetActive(true);
    }

    // =====================================
    // VOUCHER SELECTION
    // =====================================

    private void SelectVoucher(int index)
    {
        if (selectedVouchers.Contains(index))
        {
            selectedVouchers.Remove(index);
        }
        else
        {
            if (selectedVouchers.Count >= MAX_SELECTION)
                return;

            selectedVouchers.Add(index);
        }

        UpdateVoucherAppearance();
        UpdateSelectedText();
    }

    private void UpdateVoucherAppearance()
    {
        for (int i = 0; i < vouchers.Length; i++)
        {
            bool isSelected = selectedVouchers.Contains(i);

            vouchers[i].backgroundImage.sprite =
                isSelected
                ? vouchers[i].selectedSprite
                : vouchers[i].normalSprite;

            vouchers[i].voucherImage.SetActive(true);
        }
    }

    private void UpdateSelectedText()
    {
        selectedText.text =
            "SELECTED\n" + selectedVouchers.Count + "/2";

        submitButton.interactable =
            selectedVouchers.Count == MAX_SELECTION;
    }

    // =====================================
    // SUBMIT VOUCHERS
    // =====================================

    private void SubmitVouchers()
    {
        if (selectedVouchers.Count != MAX_SELECTION)
            return;

        bool allCorrect = true;

        foreach (int index in selectedVouchers)
        {
            if (!vouchers[index].isCorrect)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            ShowCongratulationPanel();
        }
        else
        {
            ShowFailedPanel();
        }
    }

    // =====================================
    // BACK TO MAIN GAME PANEL
    // =====================================

    private void BackToMainPanel()
    {
        if (mainPanelManager != null)
        {
            mainPanelManager.MarkPaulaCompleted();
        }
    }

    // =====================================
    // TRY AGAIN
    // =====================================

    public void ResetVoucherPanel()
    {
        ResetVoucherSelection();
        ShowVoucherPanel();
    }

    private void ResetVoucherSelection()
    {
        selectedVouchers.Clear();

        UpdateVoucherAppearance();
        UpdateSelectedText();
    }

    // =====================================
    // RESET PAULA WHEN GAME RESTARTS
    // =====================================

    public void ResetPaulaProgress()
    {
        ResetVoucherSelection();
        ShowProfilePanel();
    }
}
