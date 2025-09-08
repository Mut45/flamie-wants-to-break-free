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
    
    private int groundLayerIndex;
    private int playerLayerIndex;
    private bool ignoreCollisionFlag = false;
    public PlayerFlip playerFlip;
    public PlayerController playerController;
    public Collider2D playerCollider;
    private Collider2D ladderCollider;
    public PhysicsSettings physicsSettingsValues;
    public Rigidbody2D rb;

    void Start()
    {
        groundLayerIndex = LayerMask.NameToLayer("Ground");
        playerLayerIndex = LayerMask.NameToLayer("Player");
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
            Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, true);
            playerController.SetHorizontalEnabled(false);
            playerFlip.SetFlipEnabled(false);
            ignoreCollisionFlag = true;
        }
        else
        {
            // Reduce speed as player exits the ladder
            Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, false);
            ignoreCollisionFlag = false;
            rb.gravityScale = physicsSettingsValues.gravityScale;
            playerController.SetHorizontalEnabled(true);
            playerFlip.SetFlipEnabled(true);
        }
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
    private bool IsFullyInside(Collider2D a, Collider2D b)
    {
        // Check if Collider a is fully inside of Collider b
        return b.bounds.Contains(a.bounds.min) && b.bounds.Contains(a.bounds.max);
    }  
}
