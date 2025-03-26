using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float transitionSpeed = 0.5f;

    [Header("Animation")]
    [SerializeField] private CanvasGroup dialogueCanvasGroup;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource endDialogueSound;
    private DialogueSequence currentDialogue;
    private int currentGroupIndex;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;

    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;
    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueSequence dialogue)
    {
        currentDialogue = dialogue;
        currentGroupIndex = 0;
        currentLineIndex = 0;
        IsDialogueActive = true;

        dialoguePanel.SetActive(true);
        StartCoroutine(FadeIn());

        OnDialogueStart?.Invoke();
        DisplayCurrentLine();
    }

    public void DisplayNextLine()
    {
        if (isTyping)
        {
            CompleteTyping();
            return;
        }

        currentLineIndex++;
        if (currentLineIndex >= currentDialogue.Dialogue[currentGroupIndex].Lines.Count)
        {
            currentGroupIndex++;
            currentLineIndex = 0;

            if (currentGroupIndex >= currentDialogue.Dialogue.Count)
            {
                EndDialogue();
                return;
            }
        }

        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        var currentLine = currentDialogue.Dialogue[currentGroupIndex].Lines[currentLineIndex];
        
        speakerNameText.text = currentLine.SpeakerName;
        speakerImage.sprite = currentLine.SpeakerSprite;
        
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeDialogue(currentLine.DialogueText));
    }

    private IEnumerator TypeDialogue(string text)
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = currentDialogue.Dialogue[currentGroupIndex].Lines[currentLineIndex].DialogueText;
        isTyping = false;
    }

    public void EndDialogueManually()
    {
        if (IsDialogueActive && !isTyping)
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        StartCoroutine(EndDialogueRoutine());
    }

    private IEnumerator EndDialogueRoutine()
    {
        yield return StartCoroutine(FadeOut());
        if (endDialogueSound != null)
        {
            endDialogueSound.Play();
        }
        IsDialogueActive = false;
        dialoguePanel.SetActive(false);
        OnDialogueEnd?.Invoke();
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        dialogueCanvasGroup.alpha = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            yield return null;
        }

        dialogueCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        dialogueCanvasGroup.alpha = 1f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }

        dialogueCanvasGroup.alpha = 0f;
    }

    public bool IsPanelPlayingOutroAnimation()
    {
        return dialogueCanvasGroup.alpha > 0f && dialogueCanvasGroup.alpha < 1f;
    }
}