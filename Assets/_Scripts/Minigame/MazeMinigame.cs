using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Yarn.Unity;

public class MazeMinigame : MonoBehaviour
{
    [Header("References")]
    public GameObject pushText;
    public TMP_Text remainingCounterText;
    public Tilemap tilemapCollision;
    public Tilemap tilemapPushable;
    public Tilemap tilemapNotPushable;
    public Tilemap tilemapCheckpoints;
    public TileBase checkpointTile;
    public TileBase pushableTile;
    public TileBase signTile;
    public TileBase brokenSignTile;
    public Transform playerStartPosition; // Reference to the player's initial position

    [Header("Events")]
    public UnityEvent onPuzzleCompleted; // Event triggered when the puzzle is completed
    public UnityEvent OnMazeMiniGameEnd;

    //count
    public int pushableTileCount = 0;

    static bool isPuzzleCompleted = false;
    public GameObject playerOrigin;
    PlayerEvents playerEvents;
    PlayerHandler playerHandler; // Reference to the PlayerHandler script
    Dictionary<Vector3Int, TileBase> initialTilePositions = new(); // Stores the initial positions of pushable tiles
    Dictionary<Vector3Int, TileBase> checkpointTilePositions = new(); // Stores the positions of checkpoint tile

    private void Start()
    {
        playerHandler = FindFirstObjectByType<PlayerHandler>();
        playerOrigin = GameObject.Find("Player Origin");
        playerEvents = FindFirstObjectByType<PlayerEvents>();

        // Save the initial positions of all pushable tiles
        SaveInitialTilePositions();
        UpdateTileCount();

        pushText.SetActive(false);
    }

    void UpdateTileCount()
    {
        pushableTileCount = GetAllTilesInPushableTilemap().Count;
        remainingCounterText.SetText(pushableTileCount.ToString());
    }

    List<TileBase> GetAllTilesInPushableTilemap()
    {
        List<TileBase> allTiles = new();

        BoundsInt bounds = tilemapPushable.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = tilemapPushable.GetTile(position);

                if (tile != null)
                {
                    allTiles.Add(tile);
                }
            }
        }

        return allTiles;
    }

    private void SaveInitialTilePositions()
    {
        // Save pushable tile positions
        BoundsInt pushableBounds = tilemapPushable.cellBounds;
        TileBase[] pushableTiles = tilemapPushable.GetTilesBlock(pushableBounds);

        for (int x = 0; x < pushableBounds.size.x; x++)
        {
            for (int y = 0; y < pushableBounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(pushableBounds.x + x, pushableBounds.y + y, 0);
                TileBase tile = pushableTiles[x + y * pushableBounds.size.x];

                if (tile == pushableTile)
                {
                    initialTilePositions[tilePosition] = tile;
                }
            }
        }

        // Save checkpoint tile positions
        BoundsInt checkpointBounds = tilemapCheckpoints.cellBounds;
        TileBase[] checkpointTiles = tilemapCheckpoints.GetTilesBlock(checkpointBounds);

        for (int x = 0; x < checkpointBounds.size.x; x++)
        {
            for (int y = 0; y < checkpointBounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(checkpointBounds.x + x, checkpointBounds.y + y, 0);
                TileBase tile = checkpointTiles[x + y * checkpointBounds.size.x];

                if (tile == checkpointTile)
                {
                    checkpointTilePositions[tilePosition] = tile;
                }
            }
        }

        Debug.Log($"Saved {initialTilePositions.Count} pushable tiles and {checkpointTilePositions.Count} checkpoint tiles.");
    }

    // public void PushAwayFromCurrentCell()
    // {
    //     Vector3Int cellPos = tilemapCollision.WorldToCell(playerOrigin.transform.position);
    //     Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

    //     foreach (Vector3Int direction in directions)
    //     {
    //         Vector3Int targetPos = cellPos + direction;

    //         if (tilemapCollision.HasTile(targetPos))
    //         {
    //             TileBase targetTile = tilemapCollision.GetTile(targetPos);
    //             if (targetTile == pushableTile)
    //             {
    //                 Vector3Int pushPos = targetPos + direction;
    //                 if (!tilemapCollision.HasTile(pushPos))
    //                 {
    //                     tilemapCollision.SetTile(pushPos, pushableTile);
    //                     tilemapCollision.SetTile(targetPos, null);
    //                 }
    //             }
    //         }
    //     }
    // }

    // USED BY UI BUTTON
    public void PlaceSignAndPushTiles()
    {
        if (PlayerHandler.IsMazeMinigameActive == false) return;

        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        Vector3Int cellPosSign = tilemapCollision.WorldToCell(playerOrigin.transform.position);

        // Place signs on broken sign tiles
        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetPos = cellPosSign + direction;
            if (tilemapCollision.HasTile(targetPos))
            {
                TileBase targetSignTile = tilemapCollision.GetTile(targetPos);
                // Place a sign if the tile is a broken sign
                if (targetSignTile == brokenSignTile)
                    tilemapCollision.SetTile(targetPos, signTile);
            }
        }

        // Push tiles and move them to the collision layer
        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetPos = cellPosSign + direction;
            if (tilemapPushable.HasTile(targetPos))
            {
                TileBase targetPushableTile = tilemapPushable.GetTile(targetPos);

                // Push the tile if it is a pushable tile
                if (targetPushableTile == pushableTile)
                {
                    Vector3Int pushPos = targetPos + direction;

                    // If the push position is empty, move the tile
                    if (!tilemapPushable.HasTile(pushPos) && !tilemapCollision.HasTile(pushPos))
                    {
                        // Move the pushable tile to the collision tilemap
                        tilemapPushable.SetTile(targetPos, null);
                        tilemapPushable.SetTile(pushPos, pushableTile);

                        //sfx
                        SoundEffectsPlayer.Instance.PlaySFX("broomSweep");
                    }
                }
            }
        }

        // Check if any pushable tile is on the same position as a checkpoint tile
        foreach (var checkpointPosition in checkpointTilePositions.Keys)
        {
            if (tilemapPushable.HasTile(checkpointPosition))
            {
                TileBase pushableTileAtCheckpoint = tilemapPushable.GetTile(checkpointPosition);
                if (pushableTileAtCheckpoint == pushableTile)
                {
                    // Move the pushable tile to the collision tilemap
                    tilemapPushable.SetTile(checkpointPosition, null);
                    tilemapNotPushable.SetTile(checkpointPosition, pushableTile);

                    // Update Counter
                    UpdateTileCount();
                }
            }
        }

        // Check puzzle completion
        CheckPuzzleCompletion();
    }

    public void CheckPuzzleCompletion()
    {
        Debug.Log("Checking puzzle completion...");
        Debug.Log($"Pushable tile count: {pushableTileCount}");

        if (pushableTileCount <= 0)
        {
            isPuzzleCompleted = true;
            CompletePuzzle();

            playerEvents.OnMazeMiniGameEnd?.Invoke();
            Debug.Log("All pushable tiles are on top of checkpoint tiles. Puzzle completed!");
        }
    }

    public void ResetPuzzle()
    {
        pushableTileCount = 0;
        isPuzzleCompleted = false;

        // Reset all tiles to their initial positions
        tilemapPushable.ClearAllTiles();
        foreach (var tileEntry in initialTilePositions)
            tilemapPushable.SetTile(tileEntry.Key, tileEntry.Value);

        // Reset all tiles in the not pushable tilemap
        tilemapNotPushable.ClearAllTiles();

        // Reset the player's position
        if (playerStartPosition != null)
            playerHandler.transform.position = playerStartPosition.position;

        UpdateTileCount();
    }

    public void RestartPuzzle()
    {
        ResetPuzzle();
        // Trigger the Unity event for puzzle completion
        onPuzzleCompleted?.Invoke();
    }

    public void CompletePuzzle() => onPuzzleCompleted?.Invoke();

    [YarnFunction("isPuzzleCompleted")]
    public static bool IsPuzzleCompleted()
    {
        return isPuzzleCompleted;
    }
}