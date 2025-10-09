using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.Win32.SafeHandles;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public enum FanDirection
{
    Left,
    Right,
    Up
}
public class FanController : MonoBehaviour
{
    // Start is called before the first frame update

    private Collider2D _triggerZone;
    public float airflowStrength = 15.0f;
    public float resistanceMultiplier = 1.5f;
    public FanDirection fanDirection = FanDirection.Right; // Expose it in the inspector for testing
    private readonly HashSet<Rigidbody2D> _enteredBodies = new();
    public AnimationCurve fallOffCurve = AnimationCurve.Linear(0, 1, 1, 0.4f); // Use a linear curve for the magnitude falloff.
    public Transform triggerZoneOrigin; // The origin point of the fan nozzle
    public Transform triggerZoneEndpoint;
    void Awake()
    {
        _triggerZone = GetComponentInChildren<Collider2D>();
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Check if the object entering is in the affected layer
        // 2. Add all the rigid bodies into the bodies private attribute
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb != null)
        {
            _enteredBodies.Add(rb);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        var rb = collision.attachedRigidbody;
        if (rb != null) _enteredBodies.Remove(rb);
    }
    private Vector2 GetAirflowDirection()
    {
        switch (fanDirection)
        {
            case FanDirection.Left:
                return Vector2.left;
            case FanDirection.Right:
                return Vector2.right;
            case FanDirection.Up:
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }
    private float CalculateFalloff(Rigidbody2D rb)
    {

        if (_triggerZone == null || fallOffCurve == null)
        {
            return 1.0f;
        }
        Vector3 origin;

        // 1. Get normalized distance from the rigid body to the origin of the trigger zone
        if (triggerZoneOrigin != null)
        {
            origin = triggerZoneOrigin.position;
        }
        else
        {
            origin = transform.position;
        }
        Vector2 rigidbodyCenterOfMass = rb.worldCenterOfMass;
        float distanceFromOrigin = Vector2.Distance(rigidbodyCenterOfMass, origin);

        // 2. Get the estimated maximum distance from the origin to the end point of the trigger box
        // Since the extent is half of the size of 
        float maximumDistance = distanceFromOrigin > Vector2.Distance(triggerZoneEndpoint.position, origin) ? distanceFromOrigin : Vector2.Distance(triggerZoneEndpoint.position, origin);

        // 3. Evaluate the fall off based on the distance and the fall off curve
        float distanceNormalized = Mathf.Clamp01(distanceFromOrigin / maximumDistance);
        return Mathf.Max(0f, fallOffCurve.Evaluate(distanceNormalized));
    }
    void FixedUpdate()
    {
        Vector2 airflowDirection = GetAirflowDirection();
        if (airflowDirection.sqrMagnitude < 1e-6f) return;
        airflowDirection.Normalize();
        // Calculate the force to be added to each affected rigid body. Force = direction vector * strength * falloff
        foreach (var rb in _enteredBodies)
        {
            if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic)
            {
                continue;
            }
            float fallOff = CalculateFalloff(rb);
            Vector2 force = airflowDirection * airflowStrength * fallOff;

            // Adding resistance if the character/movable object is moving against the wind
            float resistanceStrentgh = Vector2.Dot(rb.velocity, airflowDirection);
            if (resistanceStrentgh < 0f)
            {
                force += airflowDirection * (-resistanceStrentgh) * resistanceMultiplier;
            }
            rb.AddForce(force, ForceMode2D.Force);
        }
    }
}
