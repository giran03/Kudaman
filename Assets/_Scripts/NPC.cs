using UnityEngine;

public class NPC : MonoBehaviour
{
    public string characterName = "";
    public string talkToNode = "";
    [SerializeField] GameObject bubble_questionMark;

    void Start()
    {
        Disable_SpriteQuestionMark();
    }

    public void Enable_SpriteQuestionMark()
    {
        if (bubble_questionMark != null)
            bubble_questionMark.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void Disable_SpriteQuestionMark()
    {
        if (bubble_questionMark != null)
            bubble_questionMark.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Disable_QuestionMarkBubble() => bubble_questionMark?.SetActive(false);
    public void Enable_QuestionMarkBubble() => bubble_questionMark?.SetActive(true);
}
