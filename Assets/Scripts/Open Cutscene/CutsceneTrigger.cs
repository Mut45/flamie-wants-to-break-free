using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTriggerController : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    private bool played = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (played == true) return;
        if (collision.CompareTag("Player"))
        {
            played = true;
            collision.GetComponent<PlayerController>().SetInputLocked(true);
            director.Play();
        }
    }
}
