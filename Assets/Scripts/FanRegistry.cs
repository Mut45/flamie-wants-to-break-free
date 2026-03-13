using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FanRegistry : MonoBehaviour
{
    public static FanRegistry Instance;
    private static readonly List<FanController> _fans = new();
    public List<FanController> Fans => _fans;
    void Awake()
    {
        Instance = this;
    }
    public void Register(FanController fan)
    {
        if (fan == null) return;
        if (!_fans.Contains(fan)) _fans.Add(fan);
    }

    public void Unregister(FanController fan)
    {
        if (fan == null) return;
        _fans.Remove(fan);
    }
}
