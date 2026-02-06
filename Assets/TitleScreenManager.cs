using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private CandleChallengeController flameToLit;
    [SerializeField] private TMP_Text startText;

    public void EnableStartButtonAndFlameAnimation()
    {
        flameToLit.ifLit = true;
        startText.color = new Color(255,255,255);
    }

}
