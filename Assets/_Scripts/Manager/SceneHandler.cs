using EasyTransition;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SceneHandler : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] TransitionSettings transitionSettings;
    [SerializeField] float transitionDelay;

    public UnityEvent onSceneChange;
    public string previousScene;

    public static SceneHandler Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        MusicManager.Instance.PlayMusic(sceneName);

        PlayerPrefs.SetString("previousScene", SceneManager.GetActiveScene().name);
        TransitionManager.Instance().Transition(sceneName, transitionSettings, transitionDelay);
        onSceneChange?.Invoke();
    }

    [YarnCommand("NextScene")]
    public void NextSceneDialogue(string sceneName)
    {
        LoadSceneWithTransition(sceneName);
    }
}
