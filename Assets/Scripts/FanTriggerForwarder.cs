using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTriggerForwarder : MonoBehaviour
{
    public FanController target; // assign the parent FanController in the inspector
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null)
            target.OnChildTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (target != null)
            target.OnChildTriggerExit2D(other);
    }
}
