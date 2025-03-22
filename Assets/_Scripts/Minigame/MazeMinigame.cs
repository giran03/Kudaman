using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class MazeMinigame : MonoBehaviour
{
    [Header("References")]
    public Tilemap tilemapCollision;
    public Tilemap tilemapPushable;
    public TileBase pushableTile;
    public TileBase signTile;
    public TileBase brokenSignTile;
    public Transform playerStartPosition; // Reference to the player's initial position

    [Header("Events")]
    public UnityEvent onPuzzleCompleted; // Event triggered when the puzzle is completed

    GameObject playerOrigin;
    PlayerHandler playerHandler; // Reference to the PlayerHandler script
    private Dictionary<Vector3Int, TileBase> initialTilePositions = new(); // Stores the initial positions of pushable tiles

    private void Start()
    {
        playerHandler = FindFirstObjectByType<PlayerHandler>();
        playerOrigin = GameObject.Find("Player Origin");

        // Save the initial positions of all pushable tiles
        SaveInitialTilePositions();
    }

    /// <summary>
    /// Saves the initial positions of all pushable tiles in the scene.
    /// </summary>
    private void SaveInitialTilePositions()
    {
        BoundsInt bounds = tilemapPushable.cellBounds;
        TileBase[] allTiles = tilemapPushable.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(bounds.x + x, bounds.y + y, 0);
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile == pushableTile)
                {
                    initialTilePositions[tilePosition] = tile;
                }
            }
        }
    }

    public void PushAwayFromCurrentCell()
    {
        Vector3Int cellPos = tilemapCollision.WorldToCell(playerOrigin.transform.position);
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetPos = cellPos + direction;

            if (tilemapCollision.HasTile(targetPos))
            {
                TileBase targetTile = tilemapCollision.GetTile(targetPos);
                if (targetTile == pushableTile)
                {
                    Vector3Int pushPos = targetPos + direction;
                    if (!tilemapCollision.HasTile(pushPos))
                    {
                        tilemapCollision.SetTile(pushPos, pushableTile);
                        tilemapCollision.SetTile(targetPos, null);
                    }
                }
            }
        }
    }

    public void PlaceSignAndPushTiles()
    {
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        Vector3Int cellPosSign = tilemapCollision.WorldToCell(playerOrigin.transform.position);
        Vector3Int cellPosPushables = tilemapPushable.WorldToCell(playerOrigin.transform.position);

        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetPos = cellPosSign + direction;
            if (tilemapCollision.HasTile(targetPos))
            {
                TileBase targetSignTile = tilemapCollision.GetTile(targetPos);

                // Place a sign if the tile is a broken sign
                if (targetSignTile == brokenSignTile)
                {
                    tilemapCollision.SetTile(targetPos, signTile);
                }
            }
        }

        //push tiles
        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetPos = cellPosSign + direction;
            if (tilemapPushable.HasTile(targetPos))
            {
                TileBase targetSignTile = tilemapPushable.GetTile(targetPos);
                TileBase targetPushableTile = tilemapPushable.GetTile(targetPos);

                // Push the tile if it is a pushable tile
                if (targetPushableTile == pushableTile)
                {
                    Vector3Int pushPos = targetPos + direction;
                    if (!tilemapPushable.HasTile(pushPos))
                    {
                        tilemapPushable.SetTile(pushPos, pushableTile);
                        tilemapPushable.SetTile(targetPos, null);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Resets the puzzle and the player's position to their initial states.
    /// </summary>
    public void ResetPuzzle()
    {
        // Reset all tiles to their initial positions
        tilemapPushable.ClearAllTiles();
        foreach (var tileEntry in initialTilePositions)
        {
            tilemapPushable.SetTile(tileEntry.Key, tileEntry.Value);
        }

        // Reset the player's position
        if (playerStartPosition != null)
        {
            playerHandler.transform.position = playerStartPosition.position;
        }
    }

    /// <summary>
    /// Restarts the puzzle and triggers the Unity event when the puzzle is completed.
    /// </summary>
    public void RestartPuzzle()
    {
        ResetPuzzle();

        // Trigger the Unity event for puzzle completion
        onPuzzleCompleted?.Invoke();
    }

    /// <summary>
    /// Call this method when the puzzle is completed.
    /// </summary>
    public void CompletePuzzle()
    {
        Debug.Log("Puzzle completed!");
        onPuzzleCompleted?.Invoke();
    }
}