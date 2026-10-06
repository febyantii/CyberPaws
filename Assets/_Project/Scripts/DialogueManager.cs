using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Components")]
    public GameObject dialoguePanel;
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private bool isDialogueActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Sembunyikan panel di awal
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(DisplayNextSentence);
    }

    public void StartDialogue(List<DialogueLine> lines)
    {
        dialogueQueue.Clear();

        foreach (DialogueLine line in lines)
        {
            dialogueQueue.Enqueue(line);
        }

        dialoguePanel.SetActive(true);
        isDialogueActive = true;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = dialogueQueue.Dequeue();

        nameText.text = currentLine.characterName;
        portraitImage.sprite = currentLine.characterAvatar;
        
        // Opsional: Jika karakter tidak punya avatar, sembunyikan gambar
        portraitImage.gameObject.SetActive(currentLine.characterAvatar != null);

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.sentence));
    }

    // Efek ketik teks huruf per huruf
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f); // Kecepatan ketik
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }

    private void Update()
    {
        // Tekan Spasi atau E untuk lanjut dialog jika panel aktif
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)))
        {
            DisplayNextSentence();
        }
    }
}