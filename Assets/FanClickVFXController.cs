using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class FanClickVFXController : MonoBehaviour
{
    
    private void OnEnable() => FanDirectionController.OnFanDirectionChange += HandleFanDirectionChange;
    private void OnDisable() => FanDirectionController.OnFanDirectionChange -= HandleFanDirectionChange;
    private RectTransform canvasRect;
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform indicator;
    [SerializeField] private GameObject clickVFXPrefab;
    [SerializeField] private Vector3 clickVfxOffset = new Vector3(0.2f, 0.2f, 0);

    [Header("Indicator flash tuning")]
    [SerializeField] private float padding = 50f;
    [SerializeField] private float flashDuration = 0.6f;
    [SerializeField] private float flashFrequency = 6f;

    [Header("Indicator flashing references")]
    private CanvasGroup indicatorCG;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        if (indicator != null)
        {
            indicatorCG = indicator.GetComponent<CanvasGroup>();
            if (indicatorCG != null)
            {
                indicatorCG.alpha = 0f;
            }
            
            indicator.gameObject.SetActive(false);
        }
        if (canvas != null)
            canvasRect = canvas.GetComponent<RectTransform>();
    }

    private bool IsOnScreen(Vector3 worldPos)
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(worldPos);
        //Debug.Log("[FanClickVFX] port pos:" + viewPortPos.ToString());
        if (viewPortPos.z < 0f) return false;
        return viewPortPos.x >= 0f && viewPortPos.x <= 1f && viewPortPos.y >= 0 && viewPortPos.y <= 1f;
    }
    private void HandleFanDirectionChange(FanController fc, FanDirection fanDir)
    {
        Debug.Log("[FanRegistry] The number of fans registered is:" + FanRegistry.Instance.Fans.Count);
        //Debug.Log("[FanClickVFX] Event handler called");
        Vector3 targetFanPos = fc.transform.position;
        if (cam == null || player == null || fc == null) return;
        // Debug.Log("[FanClickVFX] fc pos:" + fc.transform.position.ToString());
        if (IsOnScreen(targetFanPos))
        {
            SpawnClickVFXPrefab(fc);
            //Debug.Log("[FanClickVFX] Fan direction changed");
        }
        else
        {
            //TO-DO: add off screen edge flashes
            // - find the nearest off-screen fan
            FanController closestFan = GetClosestFan(FanRegistry.Instance.Fans);
            // Debug.Log("[FanClickVFX] Closest fan's location is:" + closestFan.transform.position);
            if (closestFan.transform.position == fc.transform.position)
            {
                Debug.Log("[FanClickVFX] found the closest off-screen fan)" + closestFan.transform.position);
                // - clamp the position at the screen bounds
                Vector3 targetToScreenPos = cam.WorldToScreenPoint(targetFanPos);
                Vector2 clampedPos = ClampToEdge(new Vector2(targetToScreenPos.x, targetToScreenPos.y), padding); // Clamped position of the closest fan in terms of screen pixels(where the indicator should be)
                Debug.Log("[FanClickVFX] Off-screen fan clamped position is" + clampedPos.ToString());
                // - translate the screen position to a canvas local space position

                if (canvas != null && cam != null && clampedPos != null)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, clampedPos, null, out Vector2 localPoint);
                    Debug.Log("[FanClickVFX] local point pos:" + localPoint);
                    // - move the edge indicator to the clamped position
                    indicator.anchoredPosition = localPoint;
                    Debug.Log($"[FanClickVFX] MOVED indicator to {indicator.anchoredPosition}");
                    // - start the indicator flashing coroutine
                    ActivateFlashCoroutine();
                }
            }
            return;
        }
    }
    private void ActivateFlashCoroutine()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCoroutine());
    }
    private IEnumerator FlashCoroutine()
    {
        indicator.gameObject.SetActive(true);

        float t = 0f;
        while (t < flashDuration)
        {
            indicatorCG.alpha = Mathf.Abs(Mathf.Sin(t * Mathf.PI * flashFrequency));
            t += Time.unscaledDeltaTime;
            yield return null; // the runnig of the code resumes here
        }
        indicatorCG.alpha = 0f;
        indicator.gameObject.SetActive(false);
        flashCoroutine = null;
        
    }
    private Vector2 ClampToEdge(Vector2 pos, float padding)
    {
        float minX = padding;
        float maxX = Screen.width - padding;
        float minY = padding;
        float maxY = Screen.height - padding;
        Vector2 clampedPos = new ();
        clampedPos.x = Mathf.Clamp(pos.x, minX, maxX);
        clampedPos.y = Mathf.Clamp(pos.y, minY, maxY);
        return clampedPos;
    }
    private FanController GetClosestFan(List<FanController> fans)
    {
        FanController closestFan = null;
        float closestDistance = float.PositiveInfinity;
        Debug.Log("[FanClickVFX] The number of registered fans is:" + fans.Count);
        foreach (FanController fan in fans)
        {
            float distance = (fan.transform.position - player.transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFan = fan;
            }
        }
        return closestFan;
    }
    private void SpawnClickVFXPrefab(FanController fc)
    {
        var vfxInstance = Instantiate(clickVFXPrefab, fc.transform.position + clickVfxOffset, Quaternion.identity);
    }
}
