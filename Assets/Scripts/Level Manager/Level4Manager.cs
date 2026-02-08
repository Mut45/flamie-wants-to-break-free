using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Manager : MonoBehaviour
{
    private bool ifLevelComplete = false;
    public DoorController doorController;
    public List<CandleChallengeController> candleObjectList;
    
    void Update()
    {
        if (ifLevelComplete) return;
        foreach(CandleChallengeController candle in candleObjectList)
        {
            if (!candle.IfLit) return;
        }
        ifLevelComplete = true;
        doorController.SetOpen(true);
    }
}
