using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanDirectionController : MonoBehaviour
{
    public FanController fanController;
    public Transform triggerZoneOrigin;
    public Transform triggerZoneEndpoint;
    public BoxCollider2D windTriggerBox;
    public Transform windTriggerZone;
    public FanDirection direction = FanDirection.Left;
    // Start is called before the first frame update

    void Start()
    {
        ApplyDirection();
    }
    public void RotateLeft()
    {
        switch (direction)
        {
            case FanDirection.Right:
                direction = FanDirection.Up;
                break;
            case FanDirection.Up:
                direction = FanDirection.Left;
                break;
            case FanDirection.Left:
                break;
        }
        ApplyDirection();
    }

    public void RotateRight()
    {
        switch (direction)
        {
            case FanDirection.Right:
                break;
            case FanDirection.Up:
                direction = FanDirection.Right;
                break;
            case FanDirection.Left:
                direction = FanDirection.Up;
                break;
        }
        ApplyDirection();
    }

    private float GetDirectionAngle()
    {
        switch (direction)
        {
            case FanDirection.Left:
                return 0f;
            case FanDirection.Up:
                return -90f;
            case FanDirection.Right:
                return 180f;
            default:
                return 0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RotateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            RotateRight();
        }
    }
    private void ApplyDirection()
    {
        // Rotate the tri
        float directionAngle = GetDirectionAngle();
        // Rotation angle in radians, right is 0 degree, up is 90 degrees, left is 180 degrees
        // float rotationAngle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        // windTriggerZone.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);
        windTriggerZone.localRotation = Quaternion.Euler(0f, 0f, directionAngle);
        triggerZoneEndpoint.localRotation = Quaternion.Euler(0f, 0f, directionAngle);
        if (fanController != null)
        {
            fanController.SetAirflowDirection(direction);
        }

    }
    private void OnDrawGizmos()
    {
        if (windTriggerZone != null)
        {
            Gizmos.color = new Color(0f, 1f, 1f, 0.3f); // cyan, semi-transparent
                                                        // Draw a wire cube around the windZone position
                                                        // Use local rotation to respect its orientation
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = windTriggerZone.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, windTriggerZone.GetComponent<BoxCollider2D>().size);
            Gizmos.matrix = oldMatrix;
        }
        if (windTriggerBox != null)
        {
            var b = windTriggerBox.bounds;
            Gizmos.color = new Color(1f, 1f, 0f, 0.6f); // yellow
            Gizmos.DrawWireCube(b.center, b.size);
        }
            Gizmos.color = Color.red;

        if (triggerZoneEndpoint != null)
        {
            // Draw a small sphere to show its position
            Gizmos.DrawSphere(triggerZoneEndpoint.position, 0.1f);

            // Optionally draw a line from the origin to it for clarity
            if (triggerZoneOrigin != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(triggerZoneOrigin.position, triggerZoneEndpoint.position);
            }
        }
    }
}
