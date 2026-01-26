using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DraggableWaterTank: MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerDragMode playerDragMode;
    [SerializeField] private IceMachineController iceMachine;
    [SerializeField] private Camera cam;
    [SerializeField] private FlashingObjectTint dragFlashTint;
    [SerializeField] private DragGhostPreview dragGhostPreview;
    [Header("Drag Parameters")]
    [SerializeField] private float followSpeed = 20f;
    private Rigidbody2D rb;
    private bool isDragging;
    private Vector2 originalPos;
    [SerializeField] private bool inPlacementArea;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if(!cam) cam = Camera.main;
    }
    void Update()
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

        Debug.Log("[Drag] clicked:" + IsClicked());
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
    void FixedUpdate()
    {
        if(!isDragging) return;
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newPos = Vector2.Lerp(rb.position, mouseWorldPos, followSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos); 
    }
    private void StartDrag()
    {
        isDragging = true;
        originalPos = rb.position;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        CursorSprite.Instance?.SetDragging();
    }

    private void EndDrag()
    {
        isDragging = false;
        if (inPlacementArea)
        {
            if (iceMachine) {
                iceMachine.OnWaterTankDelivered();
            }
            dragFlashTint.SetGlow(false);
            dragGhostPreview.Show(false);
            gameObject.SetActive(false);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.position = originalPos;
        }
    }

    private bool IsClicked()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitRb = Physics2D.GetRayIntersection(ray);
        if (hitRb) Debug.Log("Clicked a rigidbody");
        return hitRb && hitRb.rigidbody == rb;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlacementArea"))
        {
            inPlacementArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlacementArea"))
        {
            inPlacementArea = false;
        }
    }
}