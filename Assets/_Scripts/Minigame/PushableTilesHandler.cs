using UnityEngine;

public class PushableTilesHandler : MonoBehaviour
{
    GameObject pushText;

    void Awake()
    {
        pushText = GameObject.Find("PushText");
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            pushText?.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            pushText?.SetActive(false);
    }
}
