using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeMinigame : MonoBehaviour
{
    [Header("References")]
    public Tilemap tilemapCollision;
    public TileBase pushableTile;
    public TileBase signTile;
    public TileBase brokenSignTile;

    GameObject playerOrigin;
    bool canPlaceTile = true;

    PlayerHandler playerHandler; // Reference to the PlayerHandler script

    private void Start()
    {
        playerHandler = FindFirstObjectByType<PlayerHandler>();
        playerOrigin = GameObject.Find("Player Origin");
    }

    public void PlaceSign()
    {
        Vector3Int cellPos = tilemapCollision.WorldToCell(playerOrigin.transform.position);
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetPos = cellPos + direction;

            if (tilemapCollision.HasTile(targetPos))
            {
                TileBase targetTile = tilemapCollision.GetTile(targetPos);
                if (targetTile == brokenSignTile)
                {
                    tilemapCollision.SetTile(targetPos, signTile);
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
        Vector3Int cellPos = tilemapCollision.WorldToCell(playerOrigin.transform.position);
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetPos = cellPos + direction;

            if (tilemapCollision.HasTile(targetPos))
            {
                TileBase targetTile = tilemapCollision.GetTile(targetPos);

                // Place a sign if the tile is a broken sign
                if (targetTile == brokenSignTile)
                {
                    tilemapCollision.SetTile(targetPos, signTile);
                }

                // Push the tile if it is a pushable tile
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
}