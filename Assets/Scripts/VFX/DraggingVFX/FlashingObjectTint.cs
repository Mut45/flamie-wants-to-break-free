using UnityEngine;
public class FlashingObjectTint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color glowColor = new Color(1f, 0.5f, 0f, 1f);
    [SerializeField] private float pulseFrequency = 6f;
    private Color originalColor;
    private bool isPulsing = false;
    void Awake()
    {
        if(!sr) sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }
    void Update()
    {
        if (!isPulsing || !sr ) return;
        float t = (Mathf.Sin(Time.time * pulseFrequency) + 1f) * 0.5f;
        sr.color = Color.Lerp(originalColor, glowColor, t);
    } 
    public void SetGlow(bool isOn)
    {
        isPulsing = isOn;
        if (!isOn && sr) sr.color = originalColor;  
    }
}