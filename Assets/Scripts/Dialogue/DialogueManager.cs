using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueTypewriterController typewriter;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private PlayerController player;
    
    private string[] linesToType;
    private int currentIndex;
    private bool ifDialogueActive;

    void Update()
    {
        if(!ifDialogueActive) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSkipPressed();
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        dialoguePanel.SetActive(true);
        ifDialogueActive = true;
        linesToType = dialogueLines;
        typewriter.StartTyping(linesToType[currentIndex]);

        // Lock Player's input 
        player.SetInputLocked(true);
        
    }
    private void OnSkipPressed()
    {
        if (typewriter.IsTyping)
        {
            typewriter.SkipLine();
            return;
        }
        currentIndex ++;

        if(currentIndex >= linesToType.Length)
        {
            EndDialogue();
            return;
        }
        typewriter.StartTyping(linesToType[currentIndex]);

    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        ifDialogueActive = false;

        // Unlock player's input
        player.SetInputLocked(false);
    }
}
