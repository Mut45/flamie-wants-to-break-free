using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleChallengeController : MonoBehaviour
{
    [SerializeField] private TorchController correspondingTorch;
    public bool ifLit;
    [SerializeField] private GameObject litObject;

    void Awake()
    {
        if (litObject != null)
        {
            litObject.SetActive(false);
        }
        
    }
    public bool IfLit => ifLit;
    void Update()
    {
        if (ifLit) return;
        if (litObject != null && correspondingTorch != null)
        {
            if (correspondingTorch.IsOn)
            {
                ifLit = true;
                litObject.SetActive(true);
            }
        }
    }

}
