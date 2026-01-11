using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    public ScreenFader fader;
    [SerializeField] private float fadeTimer = 0.8f;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Transition(System.Action action)
    {
        //Debug.Log("[ScreenFade]FadeOut start");
        yield return fader.FadeOut(fadeTimer);
        //Debug.Log("[ScreenFade]FadeOut stop");
        action.Invoke();
        //Debug.Log("[ScreenFade]Action done; FadeIn start");
        yield return fader.FadeIn(fadeTimer);
        //Debug.Log("[ScreenFade]FadeIn done");
    }
}

