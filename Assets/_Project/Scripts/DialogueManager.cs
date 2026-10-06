using System; // Tambahkan ini di paling atas!
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
    private Action onDialogueEnd; // Penampung callback saat dialog selesai

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(DisplayNextSentence);
    }

    // Tambahkan parameter Action onEnd = null
    public void StartDialogue(List<DialogueLine> lines, Action onEnd = null)
    {
        onDialogueEnd = onEnd;
        dialogueQueue.Clear();

        foreach (DialogueLine line in lines)
        {
            dialogueQueue.Enqueue(line);
        }

        dialoguePanel.SetActive(true);
        
        // Tampilkan kalimat pertama
        DisplayNextSentence();

        // JANGAN langsung isDialogueActive = true; 
        // Tunggu 1 frame dulu biar pencetan tombol 'E' pemicu gak kebaca 2x
        StartCoroutine(EnableInputNextFrame());
    }

    // Coroutine untuk menunda input 1 frame
    IEnumerator EnableInputNextFrame()
    {
        yield return null; // Tunggu 1 frame Unity
        isDialogueActive = true;
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
        portraitImage.gameObject.SetActive(currentLine.characterAvatar != null);

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;

        // Panggil callback buat ngabarin NPC biar jalan lagi
        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }

    private void Update()
    {
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)))
        {
            DisplayNextSentence();
        }
    }
}