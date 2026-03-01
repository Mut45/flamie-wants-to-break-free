using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineFramingArea : MonoBehaviour
{
    public CinemachineVirtualCamera areaCam;
    public int activePriority = 20;
    public int inactivePriority = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        areaCam.Priority = activePriority;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        areaCam.Priority = inactivePriority;
    }
}
