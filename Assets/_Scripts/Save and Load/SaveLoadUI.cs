using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    public Button saveButton;
    public Button loadButton;
    private StoryManager storyManager;

    void Start()
    {
        storyManager = FindFirstObjectByType<StoryManager>();

        saveButton.onClick.AddListener(() => storyManager.ReachCheckpoint(storyManager.currentCheckpoint));
        loadButton.onClick.AddListener(() => storyManager.LoadGame());
    }

    private void OnDisable()
    {
        saveButton.onClick.RemoveAllListeners();
        loadButton.onClick.RemoveAllListeners();
    }
}