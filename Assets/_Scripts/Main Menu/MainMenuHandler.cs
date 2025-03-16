using EasyTransition;
using NUnit.Framework;
using Tymski;
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

    [SerializeField] SceneReference startButtonScene;
    [SerializeField] string instructionsSceneName = "Instructions";
    [SerializeField] string creditsSceneName = "Credits";

    [Header("Transition")]
    [SerializeField] TransitionSettings transitionSettings;
    [SerializeField] float transitionDelay;

    [Header("Character Customization")]
    public int selectedBaseBodyIndex;
    public int selectedHairIndex;
    public int selectedClothesIndex;

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
        // SceneManager.LoadScene(mainSceneName);

        PlayerPrefs.SetInt("selectedBaseBodyIndex", selectedBaseBodyIndex);
        PlayerPrefs.SetInt("selectedHairIndex", selectedHairIndex);
        PlayerPrefs.SetInt("selectedClothesIndex", selectedClothesIndex);

        Debug.Log($"Selected indexes: {selectedBaseBodyIndex}, {selectedHairIndex}, {selectedClothesIndex}");

        TransitionManager.Instance().Transition(startButtonScene, transitionSettings, transitionDelay);
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