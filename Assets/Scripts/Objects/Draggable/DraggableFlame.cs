using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableFlame : DraggableBase
{
    [SerializeField] private TitleScreenManager titleScreenManager;
    protected override void OnPlacedInArea()
    {
        titleScreenManager.EnableStartButtonAndFlameAnimation();
        dragFlashTint.SetGlow(false);
        dragGhostPreview.Show(false);
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
}
