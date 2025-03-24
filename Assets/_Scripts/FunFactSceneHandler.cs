using System.Collections;
using EasyTransition;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // private void Start()
    // {
    //     Debug.Log($"PlayerPrefs.GetString(previousScene): {PlayerPrefs.GetString("previousScene")}s");
    //     if (PlayerPrefs.GetString("previousScene") == "MainMenu")
    //         StartCoroutine(LoadScene("Scene_1", durationToLoadScene));
    //     else if (PlayerPrefs.GetString("previousScene") == "Scene_1")
    //         StartCoroutine(LoadScene("Scene_2", durationToLoadScene));
    //     else if (PlayerPrefs.GetString("previousScene") == "Scene_2")
    //         StartCoroutine(LoadScene("Scene_3", durationToLoadScene));
    //     else if (PlayerPrefs.GetString("previousScene") == "Scene_3")
    //         StartCoroutine(LoadScene("Scene_4", durationToLoadScene));
    //     else if (PlayerPrefs.GetString("previousScene") == "Scene_4")
    //         StartCoroutine(LoadScene("Scene_5", durationToLoadScene));
    //     else if (PlayerPrefs.GetString("previousScene") == "Scene_5")
    //         StartCoroutine(LoadScene("Scene_6", durationToLoadScene));
    //     else
    //         StartCoroutine(LoadScene("MainMenu",durationToLoadScene));
    // }

    IEnumerator LoadScene(string sceneName, float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneHandler.Instance.LoadSceneWithTransition(sceneName);
    }
}