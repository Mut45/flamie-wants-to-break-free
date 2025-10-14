using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    // Start is called before the first frame update
    private bool flipEnabled;
    private float horizontalInput;
    private bool faceRight = true;

    // Update is called once per frame
    void Start()
    {
        flipEnabled = true;
    }
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        SetupDirectionalFlipByScale();
    }
    public bool isFacingRight()
    {
        return faceRight;
    }

    public void SetFlipEnabled(bool input)
    {
        flipEnabled = input;
    }
    private void SetupDirectionalFlipByScale()
    {
        if (((horizontalInput < 0 && faceRight) || (horizontalInput > 0 && !faceRight)) && flipEnabled)
        {
            faceRight = !faceRight;
            Vector3 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
        }
    }
}
