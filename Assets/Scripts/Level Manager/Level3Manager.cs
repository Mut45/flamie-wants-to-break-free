using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    public Transform targetSP;
    public GameObject areaToEnable;
    public GameObject areaToDisable;
    public GameObject playerObject;
    
    
    public void SendToNextLevel()
    {
        if (playerObject == null) return;
        if (targetSP != null)
        {
            TransitionManager.Instance.StartCoroutine(TransitionManager.Instance.Transition(() =>{
                if (areaToEnable != null) areaToEnable.SetActive(true);
                TeleportPlayer();
                if (areaToDisable != null) areaToDisable.SetActive(false);
            }));
            
        }
    }
    private void TeleportPlayer()
    {
        if (playerObject == null)
        {
            return;
        }
        var rb = playerObject.GetComponent<Rigidbody2D>();
        Debug.Log("[Door] the rb for the playerObject is" + rb.ToString());
        if (rb)
        {
            rb.velocity = Vector2.zero;
            rb.position = targetSP.position;
        } 
        else
        {
            playerObject.transform.position = targetSP.position;
        }
    }
}
