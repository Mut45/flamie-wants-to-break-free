using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
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
    private float blackenFadeTime = 1f;
    private float disintegrateDuration = 2f;
    private bool isBurning = false;
    [Header("On-fire VFX")]
    private List<GameObject> activeFlames = new List<GameObject>();
    [SerializeField] private GameObject flameLoopPrefab;
    [SerializeField] private int maxFlameSpawnsPerCell = 3;
    private Color originalTint = Color.white;
    private readonly List<Vector3Int> ladderTilesCoordinates = new List<Vector3Int>();
    [SerializeField] private Vector2 flameSpawnJitterX = new Vector2(-0.1f, 0.1f);
    [SerializeField] private Vector2 flameSpawnJitterY = new Vector2(-0.1f, 0.1f);
    [Header("Ladder Components")]
    private readonly Dictionary<Vector3Int, TileBase> coordinateToTileDict = new Dictionary<Vector3Int, TileBase>();
    [SerializeField] private Tilemap ladderTileMap;
    [SerializeField] private TilemapRenderer ladderRenderer;
    private BoxCollider2D ladderCollider;
    [Header("Ladder Tiles Components")]
    [SerializeField] private TileBase topTile;
    [SerializeField] private TileBase bottomTile;
    [SerializeField] private TileBase[] midTiles;
    [Header("Disintegration Prefabs (Animated)")]
    [SerializeField] private GameObject disTopPrefab;
    [SerializeField] private GameObject disMidPrefab;
    [SerializeField] private GameObject disBotPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        ladderCollider = GetComponent<BoxCollider2D>();
        if (ladderTileMap != null)
        {
            originalTint = ladderTileMap.color;
        }
        InitTileMapping();
        Debug.Log("[Ladder] Ladder Tile Size: " + ladderTilesCoordinates.Count);
        // Debug.Log("[Ladder] Ladder Tile Coordinates 0" + ladderTilesCoordinates[0]);
        // Debug.Log("[Ladder] Ladder Tile Coordinates 1" + ladderTilesCoordinates[1]);
        // Debug.Log("[Ladder] Ladder Tile Coordinates 2" + ladderTilesCoordinates[2]);
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

    private void InitTileMapping()
    {
        ladderTilesCoordinates.Clear(); // ladderCells
        coordinateToTileDict.Clear(); // originalTiles
        if (ladderTileMap == null) return;
        BoundsInt bounds = ladderTileMap.cellBounds;
        foreach (var coordinate in bounds.allPositionsWithin)
        {
            var t = ladderTileMap.GetTile(coordinate);
            if (t != null)
            {
                ladderTilesCoordinates.Add(coordinate);
                coordinateToTileDict[coordinate] = t;
            }
        }
        // Sort the tiles so that it starts from bottom to top
        ladderTilesCoordinates.Sort((a, b) => a.y.CompareTo(b.y));
    }
    private void SpawnFlamePrefabs()
    {
        if (flameLoopPrefab == null || ladderTileMap == null) return;
        foreach (var crd in ladderTilesCoordinates)
        {
            int flameCount = Random.Range(2, maxFlameSpawnsPerCell);
            for (int i = 0; i < flameCount; i++)
            {
                Vector3 basePos = ladderTileMap.GetCellCenterWorld(crd);
                Vector3 flameSpawnJitterXYZ = new Vector3(
                    Random.Range(flameSpawnJitterX.x, flameSpawnJitterX.y),
                    Random.Range(flameSpawnJitterY.x, flameSpawnJitterY.y),
                    0f
                );
                Transform parent = ladderTileMap.transform;
                var flamePrefab = Instantiate(flameLoopPrefab, basePos + flameSpawnJitterXYZ, Quaternion.identity, parent);
                activeFlames.Add(flamePrefab);
            }
        }
    }
    private void ExtinguishFlames()
    {
        for (int i = 0; i < activeFlames.Count; i++)
        {
            if (activeFlames[i] != null)
            {
                Destroy(activeFlames[i]);
            }
        }
        activeFlames.Clear();
    }

    private IEnumerator BlackenLadder()
    {
        if (ladderRenderer == null) yield break;
        Color startingColor = ladderTileMap.color;
        // Debug.Log("[Ladder Burning] The color of the tile map is" + startingColor.ToString());
        Color targetColor = Color.black;
        float t = 0f;
        while (t < blackenFadeTime)
        {
            t += Time.deltaTime;
            float tRatio = Mathf.Clamp01(t / blackenFadeTime);
            ladderTileMap.color = Color.Lerp(startingColor, targetColor, tRatio);
            yield return null;
        }
        ladderTileMap.color = targetColor;
    }
    private IEnumerator SpawnDisintegrateAnimatedPrefabs()
    {
        if (ladderTileMap == null) yield break;

        // Top → bottom sort for the tile coordinates
        var ordered = new List<Vector3Int>(ladderTilesCoordinates);
        ordered.Sort((a, b) => b.y.CompareTo(a.y));

        foreach (var cell in ordered)
        {
            TileBase tile = coordinateToTileDict[cell];
            Vector3 pos = ladderTileMap.GetCellCenterWorld(cell);

            GameObject disAnimationPrefab = null;
            Debug.Log("[Ladder] Disintegrate prefab spawned");
            if (tile == topTile) disAnimationPrefab = disTopPrefab;
            else if (tile == bottomTile) disAnimationPrefab = disBotPrefab;
            else if (IsMiddleTile(tile)) disAnimationPrefab = disMidPrefab;

            if (disAnimationPrefab != null)
            {
                
                Instantiate(disAnimationPrefab, pos, Quaternion.identity);
            }

            // remove tile immediately
            ladderTileMap.SetTile(cell, null);
        }
        ladderTileMap.RefreshAllTiles();
        yield return new WaitForSeconds(disintegrateDuration);
    }
    private bool IsMiddleTile(TileBase t)
    {
        foreach (var mid in midTiles)
            if (t == mid) return true;
        return false;
    }
    private void ClearTilesAndHide()
    {
        if (ladderTileMap != null)
        {
            foreach (var tile in coordinateToTileDict)
            {
                ladderTileMap.SetTile(tile.Key, null);
            }
            ladderTileMap.RefreshAllTiles();
        }
        if (ladderRenderer != null) ladderTileMap.enabled = false;
        if (ladderCollider != null) ladderCollider.enabled = false;
    }
    private void RestoreTilesAndShow()
    {
        if (ladderTileMap != null)
        {
            foreach (var tile in coordinateToTileDict)
            {
                ladderTileMap.SetTile(tile.Key, tile.Value);
            }
            ladderTileMap.RefreshAllTiles();
        }
        if (ladderRenderer != null && ladderTileMap != null)
        {
            ladderRenderer.enabled = true;
            ladderTileMap.color = originalTint;
            
        }
        if (ladderCollider != null) ladderCollider.enabled = true;
    }
    private void StartBurning()
    {
        if (isBurning) return;
        isBurning = true;
        // StartCoroutine(BurnAndRespawnCoroutine());
        StartCoroutine(BurnFlow());
    }
    private IEnumerator BurnFlow()
    {
        // Spawning flame prefabs on the ladder
        SpawnFlamePrefabs();
        // Debug.Log("[Flaming Ladder] Burn started");
        yield return new WaitForSeconds(burnDuration);

        // Fade the ladder to black 
        if (ladderRenderer != null)
        {
            yield return StartCoroutine(BlackenLadder());
        }

        // Remove the active flames and remove the ladder object
        ExtinguishFlames();
        // ClearTilesAndHide();
        // Play disintegration animation for the ladder in place of the original sprite
        ClearTilesAndHide();
        yield return StartCoroutine(SpawnDisintegrateAnimatedPrefabs());

        
    }
    private System.Collections.IEnumerator BurnAndRespawnCoroutine()
    {
        yield return new WaitForSeconds(burnDuration);
        ladderRenderer.enabled = false;
        ladderCollider.enabled = false;
        yield return new WaitForSeconds(respawnDelay);
        ladderRenderer.enabled = true;
        ladderCollider.enabled = true;
        isBurning = false;
    }
}
