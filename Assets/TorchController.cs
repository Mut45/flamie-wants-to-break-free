using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    private bool isTorchOn = true;
    private bool ifPlayerEntered = false;
    [SerializeField] PlayerController player;
    [SerializeField] Light2D torchLight2D;

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            ifPlayerEntered = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            ifPlayerEntered = false;
        }
    }
    public void SetOn(bool ifOn)
    {
        isTorchOn = ifOn;
        if (torchLight2D != null)
        {
            torchLight2D.enabled = ifOn;
        }

    }
    private void OnInteraction()
    {
        if (!isTorchOn && player != null)
        {
            SetOn(true);
            player.StartExtinguished();
        }
        else if (isTorchOn && player != null)
        {
            SetOn(false);
        }
    }
    void Update()
    {
        if(ifPlayerEntered && Input.GetKeyDown(KeyCode.E))
        {
            OnInteraction();
        }
    }
}
