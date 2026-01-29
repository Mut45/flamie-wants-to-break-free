using System.Security;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class DraggableBase : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] protected PlayerDragMode playerDragMode;
    [SerializeField] protected Camera cam;
    [Header("Visuals")]
    [SerializeField] protected FlashingObjectTint dragFlashTint;
    [SerializeField] protected DragGhostPreview dragGhostPreview;
    [Header("Drag Parameters")]
    [SerializeField] protected float followSpeed = 20f;
    protected Rigidbody2D rb;
    protected bool isDragging;
    protected Vector2 originalPos;
    [SerializeField] protected bool inPlacementArea;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if(!cam) cam = Camera.main;
    }
    protected virtual void Update()
    {
        if (!playerDragMode) return;
    
        if(playerDragMode.InDragMode) {
            dragFlashTint.SetGlow(true);
            dragGhostPreview.Show(true);
        }
        else
        {
            dragFlashTint.SetGlow(false);
            dragGhostPreview.Show(false);
        }

        if (!isDragging && 
            playerDragMode.InDragMode &&
            Input.GetMouseButtonDown(0) &&
            IsClicked())
        {
            Debug.Log("[Drag] Start Dragging");
            StartDrag();
        }
        if (isDragging && Input.GetMouseButtonUp(0))
        {   
            EndDrag();
        }
    }

    protected virtual void FixedUpdate()
    {
        if(!isDragging) return;
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newPos = Vector2.Lerp(rb.position, mouseWorldPos, followSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos); 
    }

    protected virtual void StartDrag()
    {
        isDragging = true;
        originalPos = rb.position;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        CursorSprite.Instance?.SetDragging();
        OnDragStarted();
    }

    protected virtual void EndDrag()
    {
        isDragging = false;
        CursorSprite.Instance?.SetDragReady();
        if (inPlacementArea)
        {
            OnPlacedInArea();
            return;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.position = originalPos;
            OnDragCancelled();
        }
    }
    protected virtual bool IsClicked()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitRb = Physics2D.GetRayIntersection(ray);
        if (hitRb) Debug.Log("Clicked a rigidbody");
        return hitRb && hitRb.rigidbody == rb;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlacementArea"))
        {
            inPlacementArea = true;
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlacementArea"))
        {
            inPlacementArea = false;
        }
    }
    protected virtual void OnDragStarted(){ }
    protected virtual void OnDragCancelled(){ }
    protected abstract void OnPlacedInArea();
}