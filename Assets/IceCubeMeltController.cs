using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeMeltController : MonoBehaviour
{
    private bool isPlayerNearBy = false;
    [SerializeField] private PlayerController player;
    [SerializeField] private IceCubeController iceCube;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearBy = true;
            player = other.gameObject.GetComponent<PlayerController>();
        }
            
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearBy = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (iceCube != null && isPlayerNearBy && iceCube.IsSnapped && player && player.isOnFire)
        {
            iceCube.StartMelting();
        }
    }
}
