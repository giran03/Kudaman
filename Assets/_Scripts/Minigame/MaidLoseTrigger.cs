using UnityEngine;
using Yarn.Unity;

public class MaidLoseTrigger : MonoBehaviour
{
    static CircleCollider2D circleCollider2D;

    void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;
    }

    [YarnCommand("EnableLoseTrigger_ChaseMinigame")]
    public static void EnableLoseTrigger() => circleCollider2D.enabled = true;

    [YarnCommand("DisableLoseTrigger_ChaseMinigame")]
    public static void DisableLoseTrigger() => circleCollider2D.enabled = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            ChaseMinigame.Instance.EndMinigame(false);
    }
}