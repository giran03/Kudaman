using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string savePath;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/savegame.json"; //TODO: rename
    }

    public void SaveGame(int checkpointIndex, Vector2 playerPosition)
    {
        GameSaveData saveData = new()
        {
            checkpointIndex = checkpointIndex,
            playerPosition = playerPosition
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved: " + savePath);
    }

    public GameSaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameSaveData>(json);
        }

        Debug.LogWarning("No save file found!");
        return null;
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }
}
