using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    public bool initialState = false;
    [SerializeField]private bool isTorchOn = true;
    private bool ifPlayerEntered = false;
    [SerializeField] private bool isSpecialTorch = false;
    [SerializeField] PlayerController player;
    [SerializeField] private Light2D torchLight2D;
    [SerializeField] private GameObject particleParentObject;

    void Awake()
    {
        SetOn(initialState);
    }
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
    public bool IsOn => isTorchOn;
    public void SetOn(bool ifOn)
    {
        isTorchOn = ifOn;
        if (torchLight2D != null)
        {
            torchLight2D.enabled = ifOn;
            particleParentObject.SetActive(ifOn);
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
            if (isSpecialTorch)
            {
                player.StartIgnite();
            }
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
