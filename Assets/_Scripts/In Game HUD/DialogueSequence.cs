using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class DialogueSequence : ScriptableObject
{
    [SerializeField, Tooltip("List of dialogue groups that make up this sequence")]
    private List<DialogueLineGroup> _dialogue = new List<DialogueLineGroup>();

    public List<DialogueLineGroup> Dialogue => _dialogue;
}