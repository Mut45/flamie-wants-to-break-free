using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using Cinemachine;

public class PregameManager : MonoBehaviour
{
    [Header("Actor")]
    [SerializeField] private PlayerController player;
    [Header("Camera FX")]
    [SerializeField] private float sustainedCameraShakeDuration = 2.0f;
    [SerializeField] private SustainedCameraShake sustainedCameraShake;
    [SerializeField] private ScreenFader fader;
    [Header("Pause VFX")]
    [SerializeField] private Volume colorDesaturation;
    [SerializeField] private GameObject gamePausedText;
    [SerializeField] private GameObject keyInputPrompt;
    [SerializeField] private GameObject keySpamPrompt;
    [SerializeField] private float secondsBeforeShowInputPrompt = 3.0f;
    [SerializeField] private Rigidbody2D[] letterRigidbodies;
    [Header("Screen Shake VFX")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float shakeStrength = 0.5f;
    [SerializeField] private int spammedTimesBeforeBroken = 15;
    [Header("Jail Cell")]
    [SerializeField] private SpriteRenderer jailCellSR;
    [SerializeField] private Sprite brokenJailSprite;
    [Header("Dialogue")]
    [SerializeField] private DialogueData firstDialogueData;
    [SerializeField] private DialogueManager dialogueManager;
    [Header("Reference")]
    [SerializeField] private GameObject openingSceneGameObject;
    [SerializeField] private GameObject tutorialPrompt;
    [Header("Debug")]
    [SerializeField] private PlayableDirector fourthTimeline;

    private bool ifWaitForFirstInput = false;
    private bool ifKeyInputPromptVisible = false;
    private bool ifSpamPromptVisible = false;
    private int spammedTimesBeforeSpamPrompt = 5;
    private int spammedTimes = 0;

    void Update()
    {
        if (ifWaitForFirstInput && Input.GetKeyDown(KeyCode.F))
        {
            PlayScreenShakeFX();
            spammedTimes++;
            if (!ifKeyInputPromptVisible && spammedTimes < spammedTimesBeforeSpamPrompt)
            {
                if (keyInputPrompt != null) keyInputPrompt.SetActive(true);
                ifKeyInputPromptVisible = true;
            }
            else if(!ifSpamPromptVisible && spammedTimes < spammedTimesBeforeBroken && spammedTimes >= spammedTimesBeforeSpamPrompt)
            {
                if (keyInputPrompt != null) keyInputPrompt.SetActive(false);
                if (keySpamPrompt != null) keySpamPrompt.SetActive(true);
                ifSpamPromptVisible = true;
            }
            if (spammedTimes >= spammedTimesBeforeBroken)
            {
                ifWaitForFirstInput = false;
                StartJailBreakingSequence();
            }
        }

    } 
    public void DeactivateOpeningScene()
    {
        openingSceneGameObject?.SetActive(false);
    }
    public void StartFirstDialogue()
    {
        if (player != null) player.SetInputLocked(true);
        if (dialogueManager != null && firstDialogueData != null) dialogueManager.StartDialogue(firstDialogueData.lines, OnFirstDialogueFinished);
        if (fourthTimeline != null) fourthTimeline.Stop();
        if (openingSceneGameObject != null) openingSceneGameObject.SetActive(false);
    }
    private void SetDesaturation(bool ifOn)
    {
        colorDesaturation.weight = ifOn ? 1 : 0;
    }
    private void PlayGamePauseSequence()
    {
        // Desaturation
        SetDesaturation(true);

        // Activate Game Pause Text Prompt
        gamePausedText?.SetActive(true);
        StartCoroutine(WaitForSecondsBeforeShowInputprompt(secondsBeforeShowInputPrompt));
    }

    private void PlayScreenShakeFX()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        impulseSource.GenerateImpulseWithVelocity(dir * shakeStrength);
    }
    private IEnumerator WaitForSecondsBeforeShowInputprompt(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        keyInputPrompt.SetActive(true);
        ifWaitForFirstInput = true;
    }
    private void OnFirstDialogueFinished()
    {
        PlayGamePauseSequence();
        ifWaitForFirstInput = true;
    }


    public void StartJailBreakingSequence()
    {
        // Screen fade to black
        if (fader != null) StartCoroutine(fader.FadeIn(2f));
        // Set player lock to false
        player.SetInputLocked(false);
        // Disable the Spam prompt
        if(keyInputPrompt != null) keyInputPrompt.SetActive(false);
        if(keySpamPrompt != null) keySpamPrompt.SetActive(false);
        // Change the sprite for jail cell
        jailCellSR.sprite = brokenJailSprite;
        // Disable collider
        Collider2D collider = jailCellSR.gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
        // Change jail sorting order to be behind the player
        jailCellSR.sortingLayerName = "Background";
    
    }
    public void StartSustainedScreenShake()
    {
        player.SetInputLocked(true);
        sustainedCameraShake.Shake(sustainedCameraShakeDuration, 0.2f);
    }
    public void GamePauseBreakSequence()
    {
        foreach (Rigidbody2D rb in letterRigidbodies)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-200, 200));
        }
        SetDesaturation(false);
        player.SetInputLocked(false);
    }
    public void ShowTutorialPrompt()
    {
        tutorialPrompt.SetActive(true);
    }
    [ContextMenu("DEBUG/Start From Timeline 4")]
    private void DebugStartFromFourthTimeline()
    {
        fourthTimeline.Play();
    }
}
