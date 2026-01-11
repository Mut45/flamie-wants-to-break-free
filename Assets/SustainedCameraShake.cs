using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SustainedCameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;

    public void Shake(float shakeDuration, float impulseStrength, float interval = 0.03f)
    {
        StartCoroutine(ShakeRoutine(shakeDuration, impulseStrength, interval));
    }

    private IEnumerator ShakeRoutine(float duration, float strength, float interval)
    {
        if (impulseSource == null) yield break;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            impulseSource.GenerateImpulseWithVelocity(dir * strength);
            elapsed += interval;
            yield return new WaitForSeconds(interval);
        }
    }
}
