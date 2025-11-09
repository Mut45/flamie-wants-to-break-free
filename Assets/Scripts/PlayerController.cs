using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Player movement and collisions")]
    private Rigidbody2D rb;
    private Animator anim;
    public bool isOnFire = true;
    private bool wasOnFire = true;
    private float Move;
    private bool jumpingEnabled = true;
    private bool horizontalEnabled = true;
    private bool dustSpawnEnabled = true;
    public float speed;
    public float jumpForce;
    private float flameOffDuration = 3f;
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


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ApplyStateChange(isOnFire,false);
    }

    // Update is called once per frame
    void Update()
    {
        bool faceRight = playerFlip.isFacingRight();
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
            Debug.Log($"dir = {dir}, dustForwardOffset = {landDustFowardOffset}");
            Debug.Log("[Dust] Spawn Pos:" + dustSpawnPoint.position);
            Debug.Log("[Dust] LandDustSpawnPos:" + landDustSpawnPos);
            var landDust = Instantiate(landDustPrefab, landDustSpawnPos, Quaternion.identity);
            var sr = landDust.GetComponent<SpriteRenderer>();
            if (sr)
            {
                sr.flipX = !faceRight;
            }
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
        if (Move != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
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
        if (!isOnFire)
        {
            return;
        }
        StartCoroutine(ExtinguishCoroutine());
    }
    private System.Collections.IEnumerator ExtinguishCoroutine()
    {
        isOnFire = false;
        yield return new WaitForSeconds(flameOffDuration);
        isOnFire = true;

    }
    public bool CheckIsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, rayCastDistance, groundLayer) || Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, rayCastDistance, platformLayer))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Applies the correct Animator Override Controller (AOC) based on the player's current flame state.
    /// </summary>
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

    private void OnDrawGizmos()
    {
        // Help visualize the box cast for ray casting
        Gizmos.DrawWireCube(transform.position - transform.up * rayCastDistance, boxSize);
    }

}
