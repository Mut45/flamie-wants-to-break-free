using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cainos.LucidEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Player movement, collisions and states")]
    public bool ifStateTransitionPerm = false;
    private Rigidbody2D rb;
    private Animator anim;
    public bool isOnFire = false;
    private bool wasOnFire = true;
    private float Move;
    private bool jumpingEnabled = true;
    private bool horizontalEnabled = true;
    private bool dustSpawnEnabled = true;
    public float speed;
    public float jumpForce;
    [SerializeField] private float igniteFlameOnDuration = 10f;
    [SerializeField]private float flameOffDuration = 5f;
    public Coroutine igniteCoroutine;
    public Coroutine extinguishCoroutine;
    public PlayerFlip playerFlip;
    [Header("Player raycast related parameters")]
    public Vector2 boxSize;
    public float rayCastDistance;
    public LayerMask platformLayer;
    public LayerMask groundLayer;
    [Header("Spawning dust effect as the player jumps up and lands")]
    public GameObject jumpDustPrefab;
    public Transform dustSpawnPoint;
    public GameObject landDustPrefab;
    public float landDustFowardOffset = 0.22f;
    private bool wasGrounded;
    [Header("Animator override controller to swap between the flame on and flame off states")]
    public AnimatorOverrideController FlameOnAOC;
    public AnimatorOverrideController FlameoffAOC;
    [Header("Spawning smoke effect as the player transition between the states")]
    public GameObject transitionSmokePrefab;
    public Transform smokeSpawnPoint;
    [Header("Player lighting")]
    public Light2D playerLight;
    public float normalLightIntensity = 1.5f;
    public float dimLightIntensity = 0.3f;
    [Header("Player Invetory")]
    private PlayerInventoryManager inventory;
    [Header("Opening Scene")]
    [SerializeField] private bool isWizard = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventory = GetComponent<PlayerInventoryManager>();
        if (isWizard) return;
        ApplyStateChange(isOnFire,false);
    }

    void Update()
    {
        bool faceRight = playerFlip.isFacingRight();
        playerLight.intensity = isOnFire ? normalLightIntensity : dimLightIntensity;
        // Movement
        if (InputLocked) return; 
        if (Move != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        Move = Input.GetAxisRaw("Horizontal");
        if (horizontalEnabled)
        {
            rb.velocity = new Vector2(Move * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (isWizard) return;

        // Flame state transition
        if (isOnFire != wasOnFire)
        {
            if (smokeSpawnPoint && transitionSmokePrefab && dustSpawnEnabled)
            {
                var smoke = Instantiate(transitionSmokePrefab, smokeSpawnPoint.position, Quaternion.identity, smokeSpawnPoint);
                var smokeSR = smoke.GetComponent<SpriteRenderer>();
                if (smokeSR)
                {
                    smokeSR.flipX = !faceRight;
                }
            }
            ApplyStateChange(isOnFire, true);
            wasOnFire = isOnFire;
        }
        if (!wasGrounded && CheckIsGrounded() && dustSpawnEnabled)
        {
            float dir = playerFlip.isFacingRight() ? 1f : -1f;
            Vector3 landDustSpawnPos = dustSpawnPoint.position + new Vector3(dir * landDustFowardOffset, 0f, 0f);
            // Debug.Log($"dir = {dir}, dustForwardOffset = {landDustFowardOffset}");
            // Debug.Log("[Dust] Spawn Pos:" + dustSpawnPoint.position);
            // Debug.Log("[Dust] LandDustSpawnPos:" + landDustSpawnPos);
            var landDust = Instantiate(landDustPrefab, landDustSpawnPos, Quaternion.identity);
            var sr = landDust.GetComponent<SpriteRenderer>();
            if (sr)
            {
                sr.flipX = !faceRight;
            }
        }


        if (Input.GetButtonDown("Jump") && CheckIsGrounded() && jumpingEnabled)
        {
            rb.AddForce(new Vector2(Move * speed, jumpForce * 10));
            anim.SetTrigger("jumpPressed");
            if (jumpDustPrefab && dustSpawnPoint)
            {
                var jumpDust = Instantiate(jumpDustPrefab, dustSpawnPoint.position, Quaternion.identity);
                var sr = jumpDust.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.flipX = !faceRight;
                }
            }


        }
        if (CheckIsGrounded())
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }

        anim.SetFloat("verticalVelocity", rb.velocity.y);
        wasGrounded = CheckIsGrounded();
    }
    public void SetDustSpawnEnabled(bool input)
    {
        dustSpawnEnabled = input;
    }
    public void SetJumpingEnabled(bool input)
    {
        jumpingEnabled = input;
    }
    public void SetHorizontalEnabled(bool input)
    {
        horizontalEnabled = input;
    }
    public void StartExtinguished()
    {
        if (igniteCoroutine != null)
        {
            StopCoroutine(igniteCoroutine);
        }
        if (extinguishCoroutine != null)
        {
            StopCoroutine(extinguishCoroutine);
            extinguishCoroutine = null;
        }
        if (!ifStateTransitionPerm)
        {
            isOnFire = false;
        }
        else
        {
            // if (igniteCoroutine != null)
            // {
            //     StopCoroutine(igniteCoroutine);
            // }
            // if (extinguishCoroutine != null)
            // {
            //     StopCoroutine(extinguishCoroutine);
            //     extinguishCoroutine = null;
            // }
            StartCoroutine(ExtinguishCoroutine());
        }
        
    }
    public void ForceStopIgniteCoroutine()
    {
        if (igniteCoroutine != null)
        {
            StopCoroutine(igniteCoroutine);
        }
        igniteCoroutine = null;
    }
    
    public void StartIgnite()
    {
        // if (isOnFire) return;
        if (ifStateTransitionPerm)
        {
            isOnFire = true;
        }
        else
        {
            if (extinguishCoroutine != null)
            {
                StopCoroutine(extinguishCoroutine);
                extinguishCoroutine = null;
            }
            if (igniteCoroutine != null)
            {
                StopCoroutine(igniteCoroutine);
                igniteCoroutine = null;
            }
            igniteCoroutine = StartCoroutine(IgniteCorotine());
        }
    }
    private IEnumerator IgniteCorotine()
    {
        isOnFire = true;
        yield return new WaitForSeconds(igniteFlameOnDuration);
        isOnFire = false;
        igniteCoroutine = null;
    }
    private System.Collections.IEnumerator ExtinguishCoroutine()
    {
        isOnFire = false;
        yield return new WaitForSeconds(flameOffDuration);
        isOnFire = true;
        extinguishCoroutine = null;

    }
    public bool CheckIsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, rayCastDistance, groundLayer) || Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, rayCastDistance, platformLayer))
        {
            return true;
        }
        return false;
    }

    // Applies the correct Animator Override Controller (AOC) based on the player's current flame state.
    private void ApplyStateChange(bool isOnFire, bool ifStartFromTop)
    {
        if (!ifStartFromTop)
        {
            anim.runtimeAnimatorController = isOnFire ? FlameOnAOC : FlameoffAOC;
            return;
        }
        var info = anim.GetCurrentAnimatorStateInfo(0);
        float t = Mathf.Repeat(info.normalizedTime, 1f);
        int stateHash = info.shortNameHash;
        anim.runtimeAnimatorController = isOnFire ? FlameOnAOC : FlameoffAOC;
        anim.Play(stateHash, 0, t);
    }
    // Controls the input lock logic for the player drag mode.
    public bool InputLocked { get; private set; }
    public void SetInputLocked(bool locked)
    {
        InputLocked = locked;
        if (locked) GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    private void OnDrawGizmos()
    {
        // Help visualize the box cast for ray casting
        Gizmos.DrawWireCube(transform.position - transform.up * rayCastDistance, boxSize);
    }

}
