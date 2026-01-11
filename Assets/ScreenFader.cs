using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image overlayImage;
    public IEnumerator FadeIn(float duration)
    {
        Debug.Log($"FadeOut duration = {duration}");
        yield return Fade(1, 0, duration);
    }
    public IEnumerator FadeOut(float duration)
    {
        yield return Fade(0, 1, duration);
    }
    private IEnumerator Fade(float alphaFrom, float alphaTo, float duration)
    {
        float t = 0f;
        Color c = overlayImage.color;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(alphaFrom, alphaTo, t / duration);
            overlayImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        overlayImage.color = new Color(c.r, c.g, c.b, alphaTo);
    }
}
