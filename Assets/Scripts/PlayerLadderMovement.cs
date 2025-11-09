using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderMovement : MonoBehaviour
{
    private float verticalInput;
    private float speed = 2f;
    private bool isLadder = false;
    private bool isClimbing = false;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject platformObject;

    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string platformLayerName = "Platform";
    private int platformLayerIndex = -1;
    private int playerLayerIndex = -1;
    public PlayerFlip playerFlip;
    public PlayerController playerController;
    public Collider2D playerCollider;
    private Collider2D ladderCollider;
    public PhysicsSettings physicsSettingsValues;
    private bool ignorePlatformCollision = false;
    public Rigidbody2D rb;

    void Start()
    {
        playerLayerIndex = LayerMask.NameToLayer(playerLayerName);
        platformLayerIndex = LayerMask.NameToLayer(platformLayerName);
        Debug.Log("[LadderMovement] platformLayerIndex: "+ playerLayerIndex);
        Debug.Log("[LadderMovement] platformLayerIndex: "+ platformLayerIndex);
    }
    void Update()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        if (isLadder && Math.Abs(verticalInput) > 0 && IsFullyInside(playerCollider, ladderCollider))
        {
            isClimbing = true;
        }

    }
    void FixedUpdate()
    {
        if (isClimbing && IsFullyInside(playerCollider, ladderCollider))
        {
            Debug.Log("Horizontal Disabled");
            // Disable gravity when on ladder
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0, verticalInput * speed);
            bool isGoingUpDown = verticalInput < -0.1f || verticalInput > 0.1f;
            Debug.Log("[LadderMovement] isGoingdown: "+isGoingUpDown);
            TogglePlatformCollision(isGoingUpDown); 
            Debug.Log("[LadderMovement] ignoringCollision: "+ ignorePlatformCollision);
            //Physics2D.IgnoreLayerCollision(playerLayerIndex, platformLayerIndex, true);
            playerController.SetHorizontalEnabled(false);
            playerController.SetDustSpawnEnabled(false);
            playerFlip.SetFlipEnabled(false);
        }
        else
        {
            // Reduce speed as player exits the ladder
            //Physics2D.IgnoreLayerCollision(playerLayerIndex, platformLayerIndex, false);
            rb.gravityScale = physicsSettingsValues.gravityScale;
            TogglePlatformCollision(false);
            playerController.SetHorizontalEnabled(true);
            playerController.SetDustSpawnEnabled(true);
            playerFlip.SetFlipEnabled(true);
        }
    }

    private void TogglePlatformCollision(bool ignore)
    {
        // If layers are not properly set, do nothing
        if (playerLayerIndex < 0 || platformLayerIndex < 0)
            return;

        if (ignorePlatformCollision == ignore)
            return;

        Physics2D.IgnoreLayerCollision(playerLayerIndex, platformLayerIndex, ignore);
        ignorePlatformCollision = ignore;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isLadder = true;
            ladderCollider = other;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    void OnDisable()
    {
        TogglePlatformCollision(false);
    }
    private bool IsFullyInside(Collider2D a, Collider2D b)
    {
        if (a == null || b == null) return false;
        // Check if Collider a is fully inside of Collider b
        return b.bounds.Contains(a.bounds.min) && b.bounds.Contains(a.bounds.max);
    }  
}
