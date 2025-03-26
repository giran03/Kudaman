using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [SerializeField, Tooltip("The character name who speaks this line")]
    private string speakerName;

    [SerializeField, TextArea(3, 10), Tooltip("The dialogue text content")]
    private string dialogueText;

    [SerializeField, Tooltip("The character sprite/image to display")]
    private Sprite speakerSprite;

    public string SpeakerName => speakerName;
    public string DialogueText => dialogueText;
    public Sprite SpeakerSprite => speakerSprite;
}

[Serializable]
public class DialogueLineGroup
{
    [SerializeField, Tooltip("List of dialogue lines in this group")]
    private List<DialogueLine> lines = new List<DialogueLine>();

    public List<DialogueLine> Lines => lines;
}