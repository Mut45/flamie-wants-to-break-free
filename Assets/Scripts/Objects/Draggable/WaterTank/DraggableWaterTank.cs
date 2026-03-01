using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DraggableWaterTank: DraggableBase
{
    [Header("Reference")]
    [SerializeField] private IceMachineController iceMachine;

    protected override void OnPlacedInArea()
    {
        if (iceMachine) {
            iceMachine.OnWaterTankDelivered();
        }
        dragFlashTint.SetGlow(false);
        dragGhostPreview.Show(false);
        gameObject.SetActive(false);
    }
}