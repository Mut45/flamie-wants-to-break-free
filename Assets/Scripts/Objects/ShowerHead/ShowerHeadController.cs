using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerHeadController : MonoBehaviour
{
    // Start is called before the first frame update
    public float onDuration = 2.0f;
    public float offDuration = 2.0f;
    public bool isOn { get; private set; }
    public Collider2D waterTriggerBox;
    public GameObject waterVFX;
    private Coroutine loopCoroutine;

    void OnEnable()
    {
        loopCoroutine = StartCoroutine(ShowerHeadLoopCoroutine());
    }
    void OnDisable()
    {
        if (loopCoroutine != null)
        {
            StopCoroutine(ShowerHeadLoopCoroutine());
        }
    }
    IEnumerator ShowerHeadLoopCoroutine()
    {
        SetOn(true);
        while (true)
        {
            if (isOn)
            {
                yield return new WaitForSeconds(onDuration);
            }
            else
            {
                yield return new WaitForSeconds(offDuration);
            }
            SetOn(!isOn);
        }

    }
    void SetOn(bool input)
    {
        isOn = input;
        if (waterVFX) waterVFX.SetActive(isOn);
        if (waterTriggerBox) waterTriggerBox.enabled = isOn;
    }
}
