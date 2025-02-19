using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

//TODO: ADD CHECKER ON NPC NAME TO FIND WHAT DIALOGUES TO PLAY

public class DialogueConfigsHandler : MonoBehaviour
{
    [Header("Configs")]
    static Image speakerImageDisplay;

    [Header("Character Images")]
    static Sprite[] characterSprites;

    [SerializeField] Image _speakerImageDisplay;
    [SerializeField] Sprite[] _characterSprites;

    void Start()
    {
        speakerImageDisplay = _speakerImageDisplay;
        characterSprites = _characterSprites;

        Debug.Log($"characterSprites: {characterSprites.Length}");
    }

    [YarnCommand("ChangeSpeakerImage")]
    public static void ChangeSpeakerImage(string speakerName)
    {
        foreach (Sprite sprite in characterSprites)
        {
            Debug.Log($"sprite.name: {sprite.name}");
            Debug.Log($"speakerName: {speakerName}");

            if (sprite.name == speakerName)
            {
                speakerImageDisplay.sprite = sprite;
                break;
            }
            else
            {
                speakerImageDisplay.sprite = null;
                Debug.LogWarning("Speaker image not found!");
            }
        }
    }
}