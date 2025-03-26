using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class DialogueSequence : ScriptableObject
{
    [SerializeField, Tooltip("List of dialogue groups that make up this sequence")] private AudioClip voiceOver;
    [SerializeField] private List<DialogueLineGroup> _dialogue = new List<DialogueLineGroup>();

    public List<DialogueLineGroup> Dialogue => _dialogue;
    public AudioClip VoiceOver => voiceOver;
}