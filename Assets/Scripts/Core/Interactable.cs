using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected KeyCode interactKey = KeyCode.E;
    [SerializeField] protected Transform interactPromptAnchor;
    protected GameObject player;
    protected bool ifPlayerEntered;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        player = collision.gameObject;
        ifPlayerEntered = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        ifPlayerEntered = false;
        player = null;
    }

    protected virtual void OnInteraction(){}
    protected virtual void Update()
    {
        if (!ifPlayerEntered) return;
        if (Input.GetKeyDown(interactKey))
            OnInteraction();
    }


}