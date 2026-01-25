using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPopupController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer iconSR;
    public void SetIcon(Sprite sprite)
    {
        iconSR.sprite = sprite;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
