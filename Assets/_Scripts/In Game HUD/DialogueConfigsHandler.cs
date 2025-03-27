using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

//TODO: ADD CHECKER ON NPC NAME TO FIND WHAT DIALOGUES TO PLAY

public class DialogueConfigsHandler : MonoBehaviour
{
    [Header("Configs")]
    static Image speakerImageDisplay;
    [SerializeField] Image _speakerImageDisplay;

    [Header("Character Images")]
    [SerializeField] GameObject _characterImageHolder;
    static GameObject staticCharacterImageHolder;
    [SerializeField] Sprite[] _characterSprites;
    static Sprite[] characterSprites;

    void Awake()
    {
        speakerImageDisplay = _speakerImageDisplay;
        characterSprites = _characterSprites;
        staticCharacterImageHolder = _characterImageHolder;
    }

    /// <summary>
    /// Changes the speaker image based on the speaker name
    /// </summary>
    /// <param name="speakerName">current speaker/character name. Must have same name as the sprite</param>
    [YarnCommand("ChangeSpeakerImage")]
    public static void ChangeSpeakerImage(string speakerName)
    {
        speakerImageDisplay.sprite = characterSprites.FirstOrDefault(sprite => sprite.name == speakerName);
    }

    [YarnCommand("ShowImageHolder")]
    public static void ShowImageHolder()
    {
        staticCharacterImageHolder.SetActive(true);
    }

    [YarnCommand("HideImageHolder")]
    public static void HideImageHolder()
    {
        staticCharacterImageHolder.SetActive(false);
    }
}