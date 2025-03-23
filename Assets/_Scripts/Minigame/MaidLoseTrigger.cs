using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class MaidLoseTrigger : MonoBehaviour
{
    CircleCollider2D circleCollider2D;

    void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;
    }

    public void EnableLooseTrigger() => circleCollider2D.enabled = true;

    public void DisableLooseTrigger() => circleCollider2D.enabled = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            ChaseMinigame.Instance.EndMinigame(false);
    }
}