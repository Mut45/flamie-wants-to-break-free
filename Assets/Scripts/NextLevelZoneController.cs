using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelZoneController : MonoBehaviour
{
    public Level3Manager levelManager;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            levelManager.SendToNextLevel();
        }
    }
}
