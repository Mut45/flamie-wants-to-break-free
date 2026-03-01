using System.Collections;
using System.Collections.Generic;
using Cainos.LucidEditor;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class OpenCutsceneController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera vcamWizard;
    [SerializeField] private CinemachineVirtualCamera vcamFamiliar;

    [Header("Dialogue")]
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private DialogueData secondDialogueData;
    [SerializeField] private DialogueData thirdDialogueData;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableDirector secondDirector;
    [SerializeField] private PlayableDirector thirdDirector;
    [SerializeField] private PlayableDirector fourthDirector;
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Fire Familiar")]
    [SerializeField] private PlayerController playerFamiliar;
    [SerializeField] private Animator familiarAnimator;
    [SerializeField] private PlayerController player;

    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject questionmarkPrefab;
    [SerializeField] private float questionmarkPrefabSpawningOffset;
    [SerializeField] ScreenFader fader;

    private bool ifPausedForInteractionPrompt = false;
    private bool ifWaitingForExplosionInteractionPrompt = false;
    private bool ifWaitingForLastExplosionInteractionPrompt = false;
    private bool hasTriggeredLandingDialogue = false;

    void Update()
    {
        // if (!ifPausedForInteractionPrompt && !hasTriggeredLandingDialogue) return;
        if (ifPausedForInteractionPrompt && Input.GetKeyDown(KeyCode.F))
        {
            ifPausedForInteractionPrompt = false;
            interactionPrompt.SetActive(false);
            secondDirector.Play();
            // Second director plays
        }
        Debug.Log("[Familiar] Is grounded:" + playerFamiliar.CheckIsGrounded());
        if (!hasTriggeredLandingDialogue && playerFamiliar != null && playerFamiliar.CheckIsGrounded())
        {
            
            hasTriggeredLandingDialogue = true;
            if (secondDirector != null) secondDirector.Stop();
            if (dialogueManager != null && secondDialogueData != null)
            {
                
                dialogueManager.StartDialogue(secondDialogueData.lines, OnLandingDialogueFinished);
            }

        }
        if (ifWaitingForExplosionInteractionPrompt && Input.GetKeyDown(KeyCode.F))
        {
            ifWaitingForExplosionInteractionPrompt = false;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
            SpawnQuestionMarkPrefab();
            if (thirdDirector != null)
            {
                thirdDirector.Play();
            }
        }
        if (ifWaitingForLastExplosionInteractionPrompt && Input.GetKeyDown(KeyCode.F))
        {
            ifWaitingForLastExplosionInteractionPrompt = false;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
            if (fourthDirector != null)
            {
                fourthDirector.Play();
            }
        }
    }
    
    public void StartOpenSceneDialouge()
    {
        director.Stop();
        dialogueManager.StartDialogue(dialogueData.lines, PauseForInteractionPrompt);
    }
    public void StartEndOfOpenSceneDialogue()
    {
        director.Stop();

    }
    public void PauseForInteractionPrompt()
    {
        //Debug.Log("Call back function called");
        if (ifPausedForInteractionPrompt) return;
        ifPausedForInteractionPrompt = true;
        interactionPrompt.SetActive(true);
    }

    public void OnLandingDialogueFinished()
    {
        ifWaitingForExplosionInteractionPrompt = true;
        if (interactionPrompt != null) interactionPrompt.SetActive(true);
    }

    public void SpawnQuestionMarkPrefab()
    {
        var pos = playerFamiliar.transform.position + new Vector3(0, questionmarkPrefabSpawningOffset, 0);
        Instantiate(questionmarkPrefab, pos, Quaternion.identity);
    }
    public void CameraSwitchToFamiliar()
    {
        vcamFamiliar.Priority = 20;
        vcamWizard.Priority = 10;
    }
    
    private void CameraSwitchToMain()
    {
        vcamFamiliar.Priority = 0;
        vcamWizard.Priority = 0;
    }
    private void SetFamiliarVisualRotation90()
    {
        if (playerFamiliar) playerFamiliar.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90f);
    }
    private void ResetFamiliarVisualRotation()
    {
        if (playerFamiliar) playerFamiliar.gameObject.transform.localRotation = Quaternion.identity;
    }
    private void PlayFireballDownStage()
    {
        var familiarRb = playerFamiliar.GetComponent<Rigidbody2D>();
        familiarRb.gravityScale = 0f;
        if (familiarAnimator) familiarAnimator.Play("FireballFalling", 0, 0f);
    }
    public void FamiliarStartFalling()
    {
        playerFamiliar.gameObject.SetActive(true);
        CameraSwitchToFamiliar();
        SetFamiliarVisualRotation90();
        PlayFireballDownStage();
        playerFamiliar.SetInputLocked(true);
    }
    public void FamiliarMidFalling()
    {
        ResetFamiliarVisualRotation();
        if (familiarAnimator) familiarAnimator.Play("FamiliarMidFalling", 0, 0f);
        // Familiar is falling set to true

    }
    public void FamiliarEndFalling()
    {
        var familiarRb = playerFamiliar.GetComponent<Rigidbody2D>();
        familiarRb.gravityScale = 1f;
        if (familiarAnimator) familiarAnimator.Play("FamiliarEndFalling", 0, 0f);
        secondDirector.Stop();
    }
    private void OnThirdDialogueEnd()
    {
        interactionPrompt.SetActive(true);
        ifWaitingForLastExplosionInteractionPrompt = true;
    }
    public void StartThirdDialogue()
    {
        if (thirdDirector != null) thirdDirector.Stop();
        if (dialogueManager != null && dialogueData != null)
        {
            dialogueManager.StartDialogue(thirdDialogueData.lines, OnThirdDialogueEnd);
        }
    }
    private void SetPlayerStateToOriginal()
    {
        player.isOnFire = false;
        player.ifStateTransitionPerm = false;
    }
    public void TransitionToNextScene()
    {
        // fader.FadeIn(4);
        // if (fourthDirector != null) fourthDirector.Stop();
        //Debug.Log("[Cutscene] Transition function called");
        CameraSwitchToMain();
        SetPlayerStateToOriginal();
    }

}
