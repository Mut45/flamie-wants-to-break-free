using System.Collections;

using UnityEngine;

public class PlugController : MonoBehaviour
{
    [SerializeField] private bool isConnected = false;
    [SerializeField] private Transform snapPoint;
    [SerializeField] private bool freezeAfterSnap = true;
    private bool isOccupied = false;

    public bool IsConnected
    {
        get => isConnected;
        set => isConnected = value;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOccupied) return;

        IceCubeController cube = other.GetComponent<IceCubeController>();
        if (cube == null) return;
        if (cube.IsSnapped) return;
        SnapCube(cube);
    }
    private void SnapCube(IceCubeController cube)
    {
        isOccupied = true;
        cube.IsSnapped = true;
        var rb = cube.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        cube.transform.position = snapPoint.position;
        if (freezeAfterSnap)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            cube.SetSnappedToPlug(this);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
