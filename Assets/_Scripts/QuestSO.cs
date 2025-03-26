using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class QuestSO : ScriptableObject
{
    public string questName;
    [TextArea(3, 10)]
    public string description;
    public bool isCompleted;
}