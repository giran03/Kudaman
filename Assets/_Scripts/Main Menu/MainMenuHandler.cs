using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    // Singleton pattern to ensure only one instance exists
    private static MainMenuHandler _instance;
    public static MainMenuHandler Instance { get => _instance; }

    [SerializeField] Button playButton;
    [SerializeField] Button instructionsButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button exitButton;

    [SerializeField] string mainSceneName = "Game";
    [SerializeField] string instructionsSceneName = "Instructions";
    [SerializeField] string creditsSceneName = "Credits";

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;

        // Initialize buttons
        SetupButtons();
    }

    private void SetupButtons()
    {
        playButton?.onClick.AddListener(PlayGame);
        instructionsButton?.onClick.AddListener(GoToInstructions);
        creditsButton?.onClick.AddListener(GoToCredits);
        exitButton?.onClick.AddListener(ExitGame);
    }

    public void PlayGame()
    {
        // Load the main game scene
        SceneManager.LoadScene(mainSceneName);
    }

    public void GoToInstructions()
    {
        SceneManager.LoadScene(instructionsSceneName);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(creditsSceneName);
    }

    public void ExitGame()
    {
        Debug.LogError($"Exit Game");
        Application.Quit();
    }
}
