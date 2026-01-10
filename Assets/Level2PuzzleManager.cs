using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2PuzzleManager : MonoBehaviour
{
    public List<CandleChallengeController> candleObjectList;
    public DoorController door;

    // Update is called once per frame
    void Update()
    {
        foreach(CandleChallengeController candle in candleObjectList)
        {
            if (!candle.IfLit) return;
        }
        door.SetOpen(true);
    }
}
