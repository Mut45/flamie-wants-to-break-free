using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player movement and collisions")]
    private Rigidbody2D rb;
    private Animator anim;
    public bool isOnFire = true;
    private float Move;
    private bool jumpingEnabled = true;
    private bool horizontalEnabled = true;
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
    public float landDustFowardOffset = 0.2f;
    private bool wasGrounded;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!wasGrounded && CheckIsGrounded())
        {
            float dir = playerFlip.isFacingRight() ? 1f : -1f;
            Vector3 landDustSpawnPos = dustSpawnPoint.position + new Vector3(dir * landDustFowardOffset, 0f, 0f);
            Debug.Log($"dir = {dir}, dustForwardOffset = {landDustFowardOffset}");
            Debug.Log("[Dust] Spawn Pos:" + dustSpawnPoint.position);
            Debug.Log("[Dust] LandDustSpawnPos:" + landDustSpawnPos);
            var landDust = Instantiate(landDustPrefab, landDustSpawnPos, Quaternion.identity);
            bool faceRight = playerFlip.isFacingRight();
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
                bool faceRight = playerFlip.isFacingRight();
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

    private void OnDrawGizmos()
    {
        // Help visualize the box cast for ray casting
        Gizmos.DrawWireCube(transform.position - transform.up * rayCastDistance, boxSize);
    }

}
