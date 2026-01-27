using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenFanArcIndicatorUI : MonoBehaviour
{

    // private void OnEnable() => FanDirectionController.OnFanDirectionChange += HandleFanDirectionChange;
    // private void OnDisable() => FanDirectionController.OnFanDirectionChange -= HandleFanDirectionChange;
    [Header("Referecnces")]
    public RectTransform root;

    [Header("Child References")]
    public Image partialArcLeft;
    public Image partialArcRight;
    public RectTransform iconRect;
    public Image iconImage;

    [Header("Visual Settings")]
    public float ringRadius = 90f;                 
    [Range(0f, 360f)] public float arcDegrees = 180f;       
    [Range(0f, 90f)] public float gapDegrees = 36f;       
    public float flashFrequency = 6f;
    [Range(0f, 1f)] public float minAlpha = 0.2f;
    [Range(0f, 1f)] public float maxAlpha = 1f;


    void Awake()
    {
        // float halfArc = arcDegrees * 0.5f;
        // float halfGap = gapDegrees * 0.5f;
        // SetupArc(partialArcLeft, halfArc, halfGap , true);
        // SetupArc(partialArcRight, halfArc, halfGap , false);
        //SetupAllIndicatorComponents(112f, 1f);
    }
    public void SetupAllIndicatorComponents(
        float directionDegree,
        float alpha
    )
    {
        root.localRotation = Quaternion.Euler(0, 0, directionDegree);
        //iconRect.anchoredPosition = Vector2.right * ringRadius;
        float halfArc = arcDegrees * 0.5f;
        float halfGap = gapDegrees * 0.5f;
        SetupArc(partialArcLeft, halfArc, halfGap , true);
        SetupArc(partialArcRight, halfArc, halfGap , false);
        iconRect.localRotation = Quaternion.Euler(0, 0, 90 - directionDegree);
    }
    // private void SetupAllIndicatorComponents(
    //     RectTransform ring,
    //     Image partialArcLeft,
    //     Image partialArcRight,
    //     RectTransform iconRect,
    //     Image iconImage,
    //     float directionDegree,
    //     float alpha
    // )
    // {   
    //     ring.localRotation = Quaternion.Euler(0, 0, directionDegree);
        
    //     iconRect.anchoredPosition = Vector2.right * ringRadius;
    //     float halfArc = arcDegrees * 0.5f;
    //     float halfGap = gapDegrees * 0.5f;

    //     //SetupArc(partialArcLeft, -halfArc, -halfGap);
    //     //SetupArc(partialArcRight, halfGap, halfArc);

    //     SetAlpha(partialArcLeft, alpha);
    //     SetAlpha(partialArcRight, alpha);

    //     SetAlpha(iconImage, alpha);
        
    // }

    private void SetupArc(Image image, float arcDegree, float gapDegree, bool ifLeft)
    {
        float degreeSpan = arcDegree - gapDegree;
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillOrigin = 2;
        image.fillClockwise = true;
        image.fillAmount = Mathf.Clamp01(degreeSpan/360f);
        if (ifLeft)
        {
            image.rectTransform.localRotation = Quaternion.Euler(0, -180f, -gapDegree);
        }
        else
        {
            image.rectTransform.localRotation = Quaternion.Euler(0, 0f, -gapDegree);
        }

    }

    private void SetAlpha(Image image, float alpha)
    {
        if(!image) return;
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}