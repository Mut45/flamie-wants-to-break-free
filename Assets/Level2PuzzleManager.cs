using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Level2PuzzleManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    public List<CandleChallengeController> candleObjectList;
    public DoorController door;
    public CinemachineVirtualCamera gameplayCam;
    public CinemachineVirtualCamera candleCam;
    public CinemachineVirtualCamera playerCloseUpCam;
    public CinemachineImpulseSource impulseSource;
    public SustainedCameraShake sustainedShake;
    public float impulseStrength = 1f;

    [Header("Puzzle & Camera Parameters")]
    private bool ifPuzzleSolved = false;
    private Coroutine puzzleSolveCoroutine;
    [SerializeField]private float candleCamHoldDuration = 0.7f;
    [SerializeField]private float closeUpHoldBeforeShakeDuration = 1f;
    [SerializeField]private float shakeHoldDuration = 1f;
    [SerializeField]private float afterShakeDuration = 0.5f;

    void Update()
    {
        if (ifPuzzleSolved) return;
        foreach(CandleChallengeController candle in candleObjectList)
        {
            if (!candle.IfLit) return;
        }
        ifPuzzleSolved = true;
        if (puzzleSolveCoroutine != null) StopCoroutine(puzzleSolveCoroutine); 
        puzzleSolveCoroutine = StartCoroutine(PuzzleSolveSequence());
        //door.SetOpen(true);
    }

    private IEnumerator PuzzleSolveSequence()
    {
        // 1. Pan to the candles
        SetActiveCam(candleCam);
        // 2. Camera hold wait
        yield return new WaitForSeconds(candleCamHoldDuration);
        // 3. Unlock the door to the next level
        if (door != null) door.SetOpen(true);
        // 4. Pan to the player
        SetActiveCam(playerCloseUpCam);
        yield return new WaitForSeconds(closeUpHoldBeforeShakeDuration);
        // 5. Camera shake + player igniting (turning into perm transtioning state)

        //impulseSource.GenerateImpulse(impulseStrength);
        sustainedShake.Shake(shakeHoldDuration, 0.5f, 0.2f);
        yield return new WaitForSeconds(shakeHoldDuration);
        player.ifStateTransitionPerm = true;
        player.ForceStopIgniteCoroutine();
        // if (player.igniteCoroutine != null) StopCoroutine(player.igniteCoroutine);
        // player.igniteCoroutine = null;
        player.StartIgnite();
        yield return new WaitForSeconds(afterShakeDuration);
        // 6. back to normal cam
        SetActiveCam(gameplayCam);
    }
    private void SetActiveCam(CinemachineVirtualCamera vcam)
    {
        if (gameplayCam != null) gameplayCam.Priority = (vcam == gameplayCam) ? 20 : 10;
        if (candleCam != null) candleCam.Priority = (vcam == candleCam) ? 20 : 10;
        if (playerCloseUpCam != null) playerCloseUpCam.Priority = (vcam == playerCloseUpCam) ? 20 : 10;
    }
}
