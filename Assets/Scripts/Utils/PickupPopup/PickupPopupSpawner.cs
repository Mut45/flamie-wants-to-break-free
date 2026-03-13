
using UnityEngine;
public class PickupPopupSpawner : MonoBehaviour
{
    public static PickupPopupSpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Spawn(GameObject prefab, Transform parent, Vector3 localOffset, Sprite icon)
    {
        GameObject popup = Instantiate(prefab, parent);
        popup.transform.localPosition = localOffset;
        PickupPopupController popupController = popup.GetComponent<PickupPopupController>();
        popupController.SetIcon(icon);
    }
}