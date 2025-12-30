using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class FanClickVFXController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private void OnEnable() => FanDirectionController.OnFanDirectionChange += HandleFanDirectionChange;
    private void OnDisable() => FanDirectionController.OnFanDirectionChange -= HandleFanDirectionChange;
    [SerializeField] private Camera cam;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject clickVFXPrefab;
    [SerializeField] private Vector3 clickVfxOffset = new Vector3(0.2f, 0.2f, 0);
    private bool IsOnScreen(Vector3 worldPos)
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(worldPos);
        Debug.Log("[FanClickVFX] port pos:" + viewPortPos.ToString());
        if (viewPortPos.z < 0f) return false;
        return viewPortPos.x >= 0f && viewPortPos.x <= 1f && viewPortPos.y >= 0 && viewPortPos.y <= 1f;
    }
    private void HandleFanDirectionChange(FanController fc, FanDirection fanDir)
    {
        Debug.Log("[FanClickVFX] Event handler called");
        if (cam == null || player == null || fc == null) return;
        Debug.Log("[FanClickVFX] fc pos:" + fc.transform.position.ToString());
        if (IsOnScreen(fc.transform.position))
        {
            SpawnClickVFXPrefab(fc);
            Debug.Log("[FanClickVFX] Fan direction changed");
        }
        else
        {
            //TO-DO: add off screen edge flashes
            return;
        }
    }
    private void SpawnClickVFXPrefab(FanController fc)
    {
        var vfxInstance = Instantiate(clickVFXPrefab, fc.transform.position + clickVfxOffset, Quaternion.identity);
    }
    // Update is called once per frame
}
