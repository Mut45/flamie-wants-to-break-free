using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PhysicsSettings", order = 1)]
public class PhysicsSettings : ScriptableObject
{
    public float gravityScale = 1f;
}
