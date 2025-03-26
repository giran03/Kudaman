using System.Collections;
using UnityEngine;

public class FunFactSceneHandler : MonoBehaviour
{
    [SerializeField] float durationToLoadScene = 3f;
    SceneHandler sceneHandler;
    private void Awake()
    {
        sceneHandler = FindFirstObjectByType<SceneHandler>();
    }

    private void Start()
    {
        StartCoroutine(LoadScene("Scene_1", durationToLoadScene));
    }

    IEnumerator LoadScene(string sceneName, float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneHandler.Instance.LoadSceneWithTransition(sceneName);
    }
}