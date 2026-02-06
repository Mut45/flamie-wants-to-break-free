using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class DialogueTypewriterController : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float charsInputPerSecs = 20f;
    private Coroutine typingCoroutine;
    private bool isTyping;
    
    void Awake()
    {
        if (!text) text = GetComponent<TMP_Text>();
    }

    public bool IsTyping => isTyping;
    public void StartTyping(string line)
    {
        StopTyping();
        typingCoroutine = StartCoroutine(LineTypingCoroutine(line));
    }

    public void StopTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = null;
        isTyping = false;
    }

    // Increase the number of visible characters until the whole line is shown
    private IEnumerator LineTypingCoroutine(string textLine)
    {
        isTyping = true;
        text.text = textLine;
        text.maxVisibleCharacters = 0;
        int totalChars = text.textInfo.characterCount;
        while (text.maxVisibleCharacters < totalChars)
        {
            text.maxVisibleCharacters ++;
            yield return new WaitForSeconds(1f / charsInputPerSecs);
        }
        isTyping = false;
    }

    public void SkipLine()
    {
        StopTyping();
        text.maxVisibleCharacters = text.textInfo.characterCount;
    }

}
