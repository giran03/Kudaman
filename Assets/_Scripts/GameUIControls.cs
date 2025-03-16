using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class GameUIControls : MonoBehaviour
{
    public static GameUIControls instance;
    PlayerHandler playerHandler;

    [Header("UI Elements")]
    [SerializeField] Button interactKeyUIButton;

    DialogueRunner dialogueRunner;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        playerHandler = FindFirstObjectByType<PlayerHandler>();
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    }

    private void OnEnable()
    {
        dialogueRunner.onDialogueStart.AddListener(DisableInteractKeyUIButton);
        dialogueRunner.onDialogueComplete.AddListener(EnableInteractKeyUIButton);
    }

    void OnDisable()
    {
        dialogueRunner.onDialogueStart.RemoveListener(DisableInteractKeyUIButton);
        dialogueRunner.onDialogueComplete.RemoveListener(EnableInteractKeyUIButton);
    }

    public void InteractKeyUIButton() => playerHandler.CheckForNearbyNPC();

    public void EnableInteractKeyUIButton() => interactKeyUIButton.gameObject.SetActive(true);

    public void DisableInteractKeyUIButton() => interactKeyUIButton.gameObject.SetActive(false);
}