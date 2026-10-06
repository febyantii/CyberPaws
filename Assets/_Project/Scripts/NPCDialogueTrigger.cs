using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public string npcName;
    public List<DialogueLine> dialogueList;

    // Fungsi ini dipanggil untuk memulai dialog
    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogueList);
    }
}