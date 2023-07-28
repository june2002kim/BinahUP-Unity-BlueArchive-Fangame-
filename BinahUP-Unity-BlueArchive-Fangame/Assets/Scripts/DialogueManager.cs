using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Script for Dialogue Management */

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;    // Queue of string to show script in order, First In First Out

    public GameObject dialogueBox;      // GameObject where text appears
    public Text nameText;               // Text for name
    public Text dialogueText;           // Text for dialogue

    // Start is called before the first frame update
    void Start()
    {
        /*
         Allocate new sentence queue
         */

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        /*
         Get Dialogue dialogue as Parameter
        And stop the game with timeScale
        And set nameText
        And clear before sentence queue
        And Enqueue new sentences from dialogue.sentences
        And Call DisplayNextSentence function
         */

        Time.timeScale = 0;

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        /*
         If it displayed every sentences in queue, call EndDialogue function and return
        if there's left sentences in queue, Dequeue new sentence and display it with start TypeSentence coroutine
         */

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        /*
         Coroutine for displaying sentences with typing effect.
        get string sentence as Parameter 
        add each char in sentence to dialogueText in order
         */

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        /*
         Let gameObject dialogueBox disappear
        and play the game with timeScale
         */

        dialogueBox.SetActive(false);
        Time.timeScale = 1;
    }
}
