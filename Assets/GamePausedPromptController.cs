using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class PregameTimelineTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;

    private bool hasPlayerEntered = false;
    // Update is called once per frame
    void Update()
    {
        if (!hasPlayerEntered) return;
        if (hasPlayerEntered)
        {
            if (timeline != null) timeline.Play();
            hasPlayerEntered = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasPlayerEntered = true;
        }
    }
}
