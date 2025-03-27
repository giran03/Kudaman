using UnityEngine;
using UnityEngine.Events;

public class QuestTriggerBox : MonoBehaviour
{
    public QuestSO quest;
    public UnityEvent onTriggerEnter;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            QuestManager.Instance.SetCurrentQuest(quest);
            QuestUI.Instance.UpdateQuestText(quest);
            onTriggerEnter.Invoke();
        }
    }
}