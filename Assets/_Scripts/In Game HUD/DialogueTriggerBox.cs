using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueTriggerBox : DialogueTrigger
{
    [SerializeField, Tooltip("Tag of the object that can trigger the dialogue (usually 'Player')")]
    private string triggerTag = "Player";

    [SerializeField, Tooltip("Should the dialogue trigger only once?")]
    private bool triggerOnce = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            if (triggerOnce)
            {
                TriggerDialogueOnce();
            }
            else
            {
                TriggerDialogue();
            }
        }
    }

    private void OnValidate()
    {
        // Ensure we have a Collider2D and it's set to trigger
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
        }
    }
}