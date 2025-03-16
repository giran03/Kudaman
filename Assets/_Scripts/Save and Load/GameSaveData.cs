using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int checkpointIndex; // Tracks the story checkpoint
    public Vector2 playerPosition; // Tracks the player's position
}
