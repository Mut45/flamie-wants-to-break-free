using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator anim;
    private float Move;
    private bool jumpingEnabled = true;
    private bool horizontalEnabled = true;
    public float speed;
    public float jumpForce;
    public Vector2 boxSize;
    public float rayCastDistance;
    public LayerMask groundLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        }
        if (Move != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }
    public void SetJumpingEnabled(bool input)
    {
        jumpingEnabled = input;
    }
    public void SetHorizontalEnabled(bool input)
    {
        horizontalEnabled = input;
    }
    public bool CheckIsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, rayCastDistance, groundLayer))
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
