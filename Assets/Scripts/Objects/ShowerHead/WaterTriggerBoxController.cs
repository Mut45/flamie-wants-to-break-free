using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTriggerBoxController : MonoBehaviour
{
    PlayerController playerController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController = collision.GetComponent<PlayerController>();
            playerController.StartExtinguished();
        }
    }
}
