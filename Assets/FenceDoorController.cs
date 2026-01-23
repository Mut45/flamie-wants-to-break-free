using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceDoorController : DoorController
{
    [SerializeField] private PlugController plug;
    [SerializeField] private GameObject fence;
    private bool isFenceUp = false;
    protected override void Update()
    {
        base.Update();
        if (isFenceUp)
        {
            return;
        }
        if (plug != null && plug.IsConnected)
        {
            isFenceUp = true;
            if(fence != null)
            {
                fence.SetActive(false);
            }
        }
    }
    protected override bool IsFenceUp()
    {
        return isFenceUp;
    }
}
