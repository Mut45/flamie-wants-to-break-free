using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeController : MonoBehaviour
{
    private bool isPlayerNearBy = false;
    private bool isSnapped = false;
    [SerializeField] private Animator iceCubeAnimator;
    [SerializeField] private PlayerController player;
    [SerializeField] private PlugController snappedToPlug;
    private bool hasMelted = false;
    public bool HasMelted
    {
        get => hasMelted;
        set => hasMelted = value;
    }
    public void OnEnable()
    {
        iceCubeAnimator = GetComponent<Animator>();
    }
    public bool IsSnapped
    {
        get => isSnapped;
        set => isSnapped = value;
    }

    public void SetSnappedToPlug(PlugController plug)
    {
        snappedToPlug = plug;
    }
    

    public void OnMeltingAnimationEnd()
    {
        hasMelted = true;
        if (snappedToPlug != null)
        {
            snappedToPlug.IsConnected = true;
        }
    }

    public void StartMelting()
    {
        if (iceCubeAnimator)
        {
            iceCubeAnimator.SetTrigger("StartMelting");
        }
        
    }

    void Update()
    {
        // if (isSnapped && isPlayerNearBy && player.isOnFire && iceCubeAnimator)
        // {
            
        // }
    }
}
