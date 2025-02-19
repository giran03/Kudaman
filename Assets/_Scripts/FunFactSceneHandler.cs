using EasyTransition;
using Tymski;
using UnityEngine;

public class FunFactSceneHandler : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] SceneReference gameScene;

    [Header("Transition")]
    [SerializeField] TransitionSettings transitionSettings;
    [SerializeField] float transitionDelay;

    private void Start()
    {
        AutoChangeScene();
    }

    public void AutoChangeScene()
    {
        Debug.Log($"Starting Auto Scene Transition to {gameScene} in {transitionDelay}");
        TransitionManager.Instance().Transition(gameScene, transitionSettings, transitionDelay);
    }
}