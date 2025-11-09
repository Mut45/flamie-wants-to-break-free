using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LadderController : MonoBehaviour
{
    [Header("Modular Ladder Parts Sprites")]

    [Header("Build Settings")]
    [Min(0)] public int middleCount = 2;

    private readonly List<SpriteRenderer> segmentRenderers = new List<SpriteRenderer>();
    private readonly List<Transform> runtimeFlameSpawns = new List<Transform>();

    private float burnDuration = 1.5f;
    private float respawnDelay = 5f;
    private bool isBurning = false;
    [Header("Ladder Segments Sprite Renderer")]
    [SerializeField] private Sprite topSprite;
    [SerializeField] private Sprite middleSprite;
    [SerializeField] private Sprite bottomSprite;
    private TilemapRenderer ladderRenderer;
    
    private BoxCollider2D ladderCollider;
    // Start is called before the first frame update
    void Start()
    {
        ladderRenderer = GetComponent<TilemapRenderer>();
        ladderCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController.isOnFire)
            {
                StartBurning();
            }
        }
    }
    private void StartBurning()
    {
        if (isBurning) return;
        isBurning = true;
        // TODO: Acivate the on-fire effect
        StartCoroutine(BurnAndRespawnCoroutine());
    }
    private System.Collections.IEnumerator BurnAndRespawnCoroutine()
    {
        yield return new WaitForSeconds(burnDuration);
        ladderRenderer.enabled = false;
        ladderCollider.enabled = false;
        //TODO: deactivate the on-fire animation
        yield return new WaitForSeconds(respawnDelay);
        ladderRenderer.enabled = true;
        ladderCollider.enabled = true;
        isBurning = false;
    }
    private float GetSpriteWorldHeight(Sprite s)
    {
        if (s == null) return 0f;
        return s.bounds.size.y;
    }
    // private SpriteRenderer CreateLadderSegments(Transform parent, Sprite sprite, Vector2 Local)
    // {
        
    // }

    // private void BuildLadderGeometry()
    // {
    //     segmentRenderers.Clear();
    //     runtimeFlameSpawns.Clear();
    //     var parent = this.transform;

    //     // Destroy all of the previously generated ladder segments
    //     var toDelete = new List<Transform>();
    //     foreach (Transform child in parent)
    //     {
    //         if (child.name.Contains("LadderSeg_"))
    //         {
    //             toDelete.Add(child);
    //         }
    //     }
    //     for (int i = 0; i < toDelete.Count; i++) DestroyImmediate(toDelete[i].gameObject);

    //     // Keep track of the current bottom of the ladder and each segment's world height
    //     float currentY = 0f;
    //     float segHTop = GetSpriteWorldHeight(topSprite);
    //     float segHMid = GetSpriteWorldHeight(middleSprite);
    //     float segHBot = GetSpriteWorldHeight(bottomSprite);
    //     // if (bottomSprite)
    //     // {
    //     //     var r = CreateSeg(parent, bottomSprite, new Vector2(0f, y), suffix:"Bottom");
    //     //     segmentRenderers.Add(r);
    //     //     runtimeFlameAnchors.Add(CreateAnchor(r.transform, "Anchor_Bottom"));
    //     //     y += segHBot;
    //     // }
    // }
}
