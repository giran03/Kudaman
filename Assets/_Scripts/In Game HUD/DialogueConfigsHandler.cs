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

    PlayerHandler playerHandler;
    string player_image_name;

    void Awake()
    {
        playerHandler = FindFirstObjectByType<PlayerHandler>();

        speakerImageDisplay = _speakerImageDisplay;
        characterSprites = _characterSprites;
        staticCharacterImageHolder = _characterImageHolder;
    }

    // [YarnCommand("ChangeSpeakerImage")]
    // public static void ChangeSpeakerImage(string speakerName)
    // {
    //     speakerImageDisplay.sprite = characterSprites.FirstOrDefault(sprite => sprite.name == speakerName);
    // }

    // CHANGE DIALOGUES BASED ON CHARACTER
    [YarnCommand("ChangeSpeakerImage")]
    public void ChangeSpeakerImage(string speakerName, string emotionSuffix = null)
    {
        player_image_name = "";

        // Debug.Log($"speakerName: {speakerName}");
        // Debug.Log($"playerHandler.DialogueSpriteToUse(): {playerHandler.DialogueSpriteToUse()}");
        // Debug.Log($"PlayerHandler.GetSelectedGender(): {playerHandler.GetSelectedGender()}");
        if (speakerName == "isPlayer")
        {
            if (playerHandler.GetSelectedGender() == "male")
            {
                if (playerHandler.DialogueSpriteToUse() == "male_1")
                {
                    player_image_name = "male-costume1-dialouge";
                }
                else if (playerHandler.DialogueSpriteToUse() == "male_2")
                {
                    player_image_name = "male-costume1-dialouge"; //TODO: CHANGE
                }
                else if (playerHandler.DialogueSpriteToUse() == "male_3")
                {
                    player_image_name = "male-costume1-dialouge"; //TODO: CHANGE
                }
            }
            else if (playerHandler.GetSelectedGender() == "female")
            {
                if (playerHandler.DialogueSpriteToUse() == "female_1")
                {
                    player_image_name = "female-costume1-dialogue";
                }
                else if (playerHandler.DialogueSpriteToUse() == "female_2")
                {
                    player_image_name = "female-costume1-dialogue"; //TODO: CHANGE
                }
                else if (playerHandler.DialogueSpriteToUse() == "female_3")
                {
                    player_image_name = "female-costume1-dialogue"; //TODO: CHANGE
                }
            }

            //check suffix
            if (emotionSuffix != null)
            {
                player_image_name += emotionSuffix;
                Debug.Log($"player image name with suffix: {player_image_name}");
            }
            else
                player_image_name += "_0";

            speakerImageDisplay.sprite = characterSprites.FirstOrDefault(sprite => sprite.name == player_image_name);
        }
        else
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