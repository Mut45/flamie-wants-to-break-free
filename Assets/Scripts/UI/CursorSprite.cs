using TMPro;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class CursorSprite : MonoBehaviour
{
    public static CursorSprite Instance { get; private set; }
    [Header("Sprite")]
    [SerializeField] private Sprite dragReadySprite; 
    [SerializeField] private Sprite draggingSprite;

    [Header("System cursor")]
    [SerializeField] private bool hideSystemCursor = true;
    private Camera cam;
    private SpriteRenderer sr;

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        DisableSpriteCursor();

    }
    void Update()
    {
        if(!sr.enabled) return;
        if(!cam) cam = Camera.main;
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        transform.position = pos;
    }
    private void EnableSpriteCursor(Sprite sprite)
    {
        Cursor.visible = false;
        sr.enabled = true;
        sr.sprite = sprite;
    }
    private void DisableSpriteCursor()
    {
        sr.sprite = null;
        Cursor.visible = true;
        sr.enabled = false;
    }
    // System cursor -> Drag Ready Sprite -> Dragging Sprite
    public void SetDragReady() {EnableSpriteCursor(dragReadySprite);}
    public void SetDragging(){EnableSpriteCursor(draggingSprite);}
    public void SetDefault(){DisableSpriteCursor();}
}