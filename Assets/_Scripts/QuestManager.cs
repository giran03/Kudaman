using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestSO> quests = new List<QuestSO>();
    public QuestSO currentQuest;
    private static QuestManager _instance;
    private int currentQuestIndex = -1;
    public bool nextQuest;

    public static QuestManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QuestManager>();
            }
            return _instance;
        }
    }

    public void SetCurrentQuest(QuestSO quest)
    {
        currentQuest = quest;
    }

    public void CompleteCurrentQuest()
    {
        if (currentQuest != null)
        {
            currentQuest.isCompleted = true;
        }
    }

    private void Update()
    {
        if (nextQuest)
        {
            nextQuest = false;
            NextQuest();
        }
    }

    public void InitializeQuests(List<QuestSO> questList)
    {
        quests = questList;
        currentQuestIndex = -1;
    }

    public void NextQuest()
    {
        if (quests == null || quests.Count == 0) return;

        currentQuestIndex++;
        if (currentQuestIndex >= quests.Count)
        {
            currentQuestIndex = -1;
            currentQuest = null;
            return;
        }

        currentQuest = quests[currentQuestIndex];
        QuestUI.Instance.UpdateQuestText(currentQuest);
    }
}