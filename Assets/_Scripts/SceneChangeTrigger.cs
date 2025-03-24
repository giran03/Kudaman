using UnityEngine;
using Yarn.Unity;

public class SceneChangeTrigger : MonoBehaviour
{
    public static SceneChangeTrigger Instance;
    [Header("Scene Settings")]
    public string sceneName; // The name of the scene to load
    public bool isEnabled;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger has the player tag
        if (collision.CompareTag("Player"))
        {
            if (isEnabled)
            {
                // Call the SceneHandler to change the scene
                SceneHandler.Instance.LoadSceneWithTransition(sceneName);
                Debug.Log($"Player entered the trigger. Changing scene to: {sceneName}");
            }
        }
    }

    [YarnCommand("EnableSceneChangeTrigger")]
    public void EnableSceneChangeTrigger()
    {
        isEnabled = true;
    }
}