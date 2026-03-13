using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainInteract : MonoBehaviour
{
    private bool isUsed = false;
    private bool ifPlayerEntered = false;
    public PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ifPlayerEntered && Input.GetKeyDown(KeyCode.E))
        {
            OnPlayerInteraction();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            ifPlayerEntered = true;
        }
    }
    private void OnPlayerInteraction()
    {
        Debug.Log("Player has interacted with the fountain");
        //TODO: Change the state of the fountain sprite to used 
        isUsed = true;
        playerHealth.Heal(1);
    }
}
