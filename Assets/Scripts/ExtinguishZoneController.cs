using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishZoneController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.ifStateTransitionPerm = false;
        }
    }
}
