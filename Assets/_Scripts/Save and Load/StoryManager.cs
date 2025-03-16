using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    private SaveManager saveManager;
    public int currentCheckpoint = 0;
    public Vector2 playerPosition;

    void Start()
    {
        saveManager = FindFirstObjectByType<SaveManager>();
        LoadGame();
    }

    public void ReachCheckpoint(int checkpoint)
    {
        currentCheckpoint = checkpoint;
        saveManager.SaveGame(currentCheckpoint, playerPosition);
    }

    public void LoadGame()
    {
        GameSaveData savedData = saveManager.LoadGame();
        if (savedData != null)
        {
            currentCheckpoint = savedData.checkpointIndex;
            playerPosition = savedData.playerPosition;
            Debug.Log("Loaded checkpoint: " + currentCheckpoint);
            // Load the corresponding scene/dialogue
        }
    }
}
