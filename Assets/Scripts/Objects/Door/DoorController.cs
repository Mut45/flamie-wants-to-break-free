using UnityEngine;

using System.Collections;



#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


public class DoorController : MonoBehaviour
{   
    [Header("Reference")]
    [SerializeField] bool isDoorOpen;
    public Transform targetSP;
    public GameObject areaToEnable;
    public GameObject areaToDisable;
    public GameObject playerObject;
    public Sprite openSprite;
    public Sprite closeSprite;
    public SpriteRenderer doorSpriteRenderer;
    private bool ifPlayerEntered = false;

    protected virtual bool IsFenceUp() => true;

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

    private void OnInteraction()
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
    public void SetOpen(bool open)
    {
        isDoorOpen = open;
    }
    protected virtual void Update()
    {
        if (!isDoorOpen)
        {
            doorSpriteRenderer.sprite = closeSprite;
            return;
        }
        else
        {
            doorSpriteRenderer.sprite = openSprite;
        }
        if(isDoorOpen && IsFenceUp() && ifPlayerEntered && Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnInteraction();
        }  
    }
}
