using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    private bool ifStarted = false;
    private bool ifStartButtonActive = false;
    [Header("Title Screen Puzzle References")]
    [SerializeField] private CandleChallengeController flameToLit;
    [SerializeField] private TMP_Text startText;

    [Header("Fade to Black Sequence")]
    [SerializeField] Image fadeToBlackImage;
    [SerializeField] float fadeDuration = 1.0f;

    [Header("Prompt for Entering Name")]
    [SerializeField] private GameObject namePanel;
    [SerializeField] private DialogueTypewriterController typewriterFX;
    [SerializeField] private TMP_Text textPrompt;
    [SerializeField] private TMP_InputField nameInput;

    public void EnableStartButtonAndFlameAnimation()
    {
        flameToLit.ifLit = true;
        startText.color = new Color(255,255,255);
        ifStartButtonActive = true;
    }

    public void OnPressStartButton()
    {
        if(ifStarted || !ifStartButtonActive) return;
        ifStarted = true;
        Debug.Log("[Start Screen] Start pressed");
        StartCoroutine(FadeFlowCoroutine());
    }

    public void OnNameSubmitted(string playerName)
    {
        PlayerPrefs.SetString("PLAYER_NAME", playerName);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("GameplayScene");
    }

    private IEnumerator FadeFlowCoroutine()
    {
        // Fade to black sequence
        yield return Fade(0f, 1f, fadeDuration);

        // Enable text prompt for entering player's name
        if(namePanel != null)
        {
            namePanel.SetActive(true);
        }
        if (textPrompt != null)
        {
            typewriterFX.StartTyping("What is your name?");
            // textPrompt.text = "What is your name?";
        }
        if (nameInput != null)
        {
            nameInput.text = "";
            nameInput.ActivateInputField();
            nameInput.Select();
        }
    }
    
    private IEnumerator Fade(float fromAlpha, float toAlpha, float duration)
    {
        if (fadeToBlackImage == null) yield break;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(fromAlpha, toAlpha, t / duration);
            SetFadeAlpha(a);
            yield return null;
        }
        SetFadeAlpha(toAlpha);
    }

    private void SetFadeAlpha(float a)
    {
        Color c = fadeToBlackImage.color;
        c.a = a;
        fadeToBlackImage.color = c;
    }

}
