using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeOnSpawn : MonoBehaviour
{
    [SerializeField] private float afterSpawnPhaseDuration = 2f;
    [SerializeField] private string tempLayerName = "Ice Cube At Spawn";
    [SerializeField] private string defaultLayerName = "Ice Cube";
    private void OnEnable()
    {
        StartCoroutine(PhaseThroughPlatformsRoutine());
    }
    private IEnumerator PhaseThroughPlatformsRoutine()
    {
        int ghostLayer = LayerMask.NameToLayer(tempLayerName);
        int normalLayer = LayerMask.NameToLayer(defaultLayerName);

        if (ghostLayer == -1 || normalLayer == -1)
        {
            Debug.LogError("IceCube layers not found. Check layer names.");
            yield break;
        }

        gameObject.layer = ghostLayer;
        yield return new WaitForSeconds(afterSpawnPhaseDuration);
        gameObject.layer = normalLayer;
    }
}
