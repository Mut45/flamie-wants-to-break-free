using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    private float burnDuration = 1.5f;
    private float respawnDelay = 5f;
    private bool isBurning = false;
    private SpriteRenderer ladderRenderer;
    private Collider2D ladderCollider;
    // Start is called before the first frame update
    void Start()
    {
        ladderRenderer = GetComponent<SpriteRenderer>();
        ladderCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController.isOnFire)
            {
                StartBurning();
            }
        }
    }
    private void StartBurning()
    {
        if (isBurning) return;
        isBurning = true;
        // TODO: Acivate the on-fire effect
        StartCoroutine(BurnAndRespawnCoroutine());
    }
    private System.Collections.IEnumerator BurnAndRespawnCoroutine()
    {
        yield return new WaitForSeconds(burnDuration);
        ladderRenderer.enabled = false;
        ladderCollider.enabled = false;
        //TODO: deactivate the on-fire animation
        yield return new WaitForSeconds(respawnDelay);
        ladderRenderer.enabled = true;
        ladderCollider.enabled = true;
        isBurning = false;
    }
}
