using UnityEngine;
public class DragGhostPreview : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ghostRenderer;
    [SerializeField] private Color ghostColor = new Color(1, 1f, 1f, 0.3f);
    [SerializeField] private float flashFrequency = 6f;

    private bool isVisible;
    private bool isValidPlacement;
    private void Awake()
    {
        if (ghostRenderer)
            ghostRenderer.enabled = false;
    }
    private void Update()
    {
        if (!isVisible || !ghostRenderer) return;
        float t = (Mathf.Sin(Time.time * flashFrequency) + 1f) * 0.5f;
        float alpha = Mathf.Lerp(0.3f, 0.6f, t);
        Color c = ghostColor;
        c.a = ghostColor.a * alpha;
        ghostRenderer.color = c;
    }
    public void Show(bool show)
    {
        if (!ghostRenderer) return;
        ghostRenderer.enabled = show;
        isVisible = show;
        isValidPlacement = false;
    }
}