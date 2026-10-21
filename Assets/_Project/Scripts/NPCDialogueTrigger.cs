using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Tambahkan ini di paling atas!

public class NPCDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Data")]
    public string npcName;
    public List<DialogueLine> dialogueList;

    [Header("Interact UI & Movement")]
    public GameObject interactPrompt;
    public MonoBehaviour movementScript;

    [Header("Minigame Scene")]
    public string minigameSceneName; // Isi nama scene minigame di sini

    private bool isPlayerInRange = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (interactPrompt != null) interactPrompt.SetActive(false);
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        if (movementScript != null) movementScript.enabled = false;
        if (rb != null) rb.velocity = Vector2.zero;

        DialogueManager.Instance.StartDialogue(dialogueList, () =>
        {
            // Pas dialog beres, langsung pindah ke scene minigame
            if (!string.IsNullOrEmpty(minigameSceneName))
            {
                SceneManager.LoadScene(minigameSceneName);
            }
            else
            {
                ResumeNPCMovement();
            }
        });
    }

    private void ResumeNPCMovement()
    {
        if (movementScript != null) movementScript.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }
}