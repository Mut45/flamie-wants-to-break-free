using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxScrollingController : MonoBehaviour
{
    private float startPos, length;
    private float lastCamX;
    public GameObject cam;
    public float parallaxEffectSpeed;


    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffectSpeed;
        float movement = cam.transform.position.x * (1 - parallaxEffectSpeed);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
    // void FixedUpdate()
    // {   
    //     float distance = cam.transform.position.x * parallexEffectSpeed;
    //     transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    //     lastCamPos = cam.transform.position.x;
    // }

    // void LateUpdate()
    // {
    //     float camX = cam.transform.position.x;
    //     float deltaX = camX - lastCamX;

    //     // Move this background layer by a fraction of the camera movement.
    //     transform.position += Vector3.right * (deltaX * parallaxEffectSpeed);

    //     lastCamX = camX;
    // }
}
