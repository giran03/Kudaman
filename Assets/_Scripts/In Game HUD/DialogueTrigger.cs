using System.Collections;
using UnityEngine;
using Cinemachine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("The data of the dialogue to display")]
    private DialogueSequence dialogueData;

    [SerializeField, Tooltip("Camera to enable when the dialogue is triggered")]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private UnityEngine.Events.UnityEvent onStartDialogue;
    [SerializeField] private UnityEngine.Events.UnityEvent onEndDialogue;

    private bool willResetTrigger = false;
    private bool isTriggered = false;

    public void SetDialogueData(DialogueSequence newDialogueData) => dialogueData = newDialogueData;

    public void TriggerDialogueOnce()
    {
        willResetTrigger = true;
        TriggerDialogue();
    }

    public void TriggerDialogue(float delay)
    {
        StartCoroutine(TriggerDialogueDelayed(delay));
    }

    private IEnumerator TriggerDialogueDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        if (isTriggered) return;

        isTriggered = true;

        if (!DialogueManager.Instance.IsDialogueActive)
        {
            DialogueManager.Instance.OnDialogueStart += OnDialogueStart;
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;

            StartCoroutine(DoTriggerDialogue());
        }
    }

    private IEnumerator DoTriggerDialogue()
    {
        yield return null;
        if (!DialogueManager.Instance.IsTyping)
        {
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
    }

    private void OnDialogueStart()
    {
        DialogueManager.Instance.OnDialogueStart -= OnDialogueStart;

        if (cinemachineVirtualCamera)
            cinemachineVirtualCamera.enabled = true;

        onStartDialogue?.Invoke();
    }

    private void OnDialogueEnd()
    {
        Debug.Log($"[{gameObject.name}] OnDialogueEnd called");
        DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;

        if (cinemachineVirtualCamera)
            cinemachineVirtualCamera.enabled = false;

        if (willResetTrigger)
        {
            isTriggered = true; // Keep it triggered permanently for one-time dialogues
        }
        else
        {
            StartCoroutine(SetUntriggeredWhenAnimationEnds());
        }

        onEndDialogue?.Invoke();
    }

    private IEnumerator SetUntriggeredWhenAnimationEnds()
    {
        Debug.Log($"[{gameObject.name}] Waiting for panel animation to end");
        yield return new WaitUntil(() => DialogueManager.Instance.IsPanelPlayingOutroAnimation());
        Debug.Log($"[{gameObject.name}] Panel animation ended, setting isTriggered to false");
        isTriggered = false;
    }
}