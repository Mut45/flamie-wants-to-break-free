using UnityEngine;
[RequireComponent(typeof(PlayerController))]
public class PlayerDragMode : MonoBehaviour
{
    public bool InDragMode { get; private set; }
    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    private void ToggleDragMode()
    {
        InDragMode = !InDragMode;
        player.SetInputLocked(InDragMode);
        Debug.Log("[DragMode] Drag mode on");
        if (InDragMode)
        {
           CursorSprite.Instance?.SetDragReady();
        } else
        {
            CursorSprite.Instance?.SetDefault();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) ToggleDragMode();
    }
    
}