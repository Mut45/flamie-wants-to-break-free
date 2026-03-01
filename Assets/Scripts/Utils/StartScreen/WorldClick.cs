using UnityEngine;
using UnityEngine.Events;
public class WorldSpaceClick: MonoBehaviour
{
    public UnityEvent onClick;
    Camera cam;
    void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 point = new Vector2(world.x, world.y);
        Collider2D hit = Physics2D.OverlapPoint(point);
        if (hit != null && hit.gameObject == gameObject)
        {
           onClick.Invoke(); 
        }

    }
}