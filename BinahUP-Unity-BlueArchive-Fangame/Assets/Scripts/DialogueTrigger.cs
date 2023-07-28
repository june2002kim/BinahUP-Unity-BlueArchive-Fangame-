using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for Triggering Dialogue */

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
