using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeMinigame : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase pushableTile;
    public TileBase placeableTile;
    public Transform player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MovePlayer(Vector3Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MovePlayer(Vector3Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MovePlayer(Vector3Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MovePlayer(Vector3Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceTile();
        }
    }

    void MovePlayer(Vector3Int direction)
    {
        Vector3Int playerPos = tilemap.WorldToCell(player.position);
        Vector3Int targetPos = playerPos + direction;
        Vector3Int pushPos = playerPos - direction;

        if (tilemap.HasTile(targetPos))
        {
            TileBase targetTile = tilemap.GetTile(targetPos);
            if (targetTile == pushableTile)
            {
                if (!tilemap.HasTile(pushPos))
                {
                    tilemap.SetTile(pushPos, pushableTile);
                    tilemap.SetTile(targetPos, null);
                }
            }
        }

        player.position = tilemap.CellToWorld(targetPos) + new Vector3(0.5f, 0.5f, 0);
    }

    void PlaceTile()
    {
        Vector3Int playerPos = tilemap.WorldToCell(player.position);
        tilemap.SetTile(playerPos, placeableTile);
    }
}