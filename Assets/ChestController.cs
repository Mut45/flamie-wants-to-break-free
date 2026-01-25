using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Interactable
{
    [SerializeField] private GameObject glow;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private GameObject pickupPopupPrefab;
    [SerializeField] private Sprite chestOpenSprite;
    [SerializeField] private Sprite chestCloseSprite;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private bool isOpen;
    protected override void OnInteraction()
    {
        if(isOpen) return;
        isOpen = true;
        sr.sprite = chestOpenSprite;
        glow.SetActive(false);
        if (pickupPopupPrefab != null && itemSprite != null && player != null)
        {
            var popup = Instantiate(pickupPopupPrefab, player.transform);
            popup.transform.localPosition = new Vector3(0f, 0.8f, 0f);
            var popupScript = popup.GetComponent<PickupPopupController>();
            if (popupScript != null) popupScript.SetIcon(itemSprite);
        }
        PlayerInventoryManager playerInvetory = player.GetComponent<PlayerInventoryManager>();
        playerInvetory.Add("Remote");
    }
}
