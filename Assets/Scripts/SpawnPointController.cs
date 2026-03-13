using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public string spawnId;
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(spawnId))
        {
            Debug.LogError(
                $"SpawnPoint on '{gameObject.name}' must have a spawnId!",
                this
            );
        }
    }
}