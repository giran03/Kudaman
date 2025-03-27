using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;

public class GeneralTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public bool triggerOnce = false;
    [Dropdown("GetTags"), SerializeField] private string triggerTag;

    private List<string> GetTags()
    {
        return UnityEditorInternal.InternalEditorUtility.tags.ToList();
    }

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered && triggerOnce) return;
        if (other.gameObject.tag != triggerTag)
        {
            Debug.Log($"OnTriggerEnter2D: {other.gameObject.name} has tag {other.gameObject.tag}, expected {triggerTag}");
            return;
        }

        Debug.Log($"OnTriggerEnter2D: {other.gameObject.name} with tag {triggerTag} triggered successfully");
        onTriggerEnter.Invoke();
        triggered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (triggered && triggerOnce) return;
        if (other.gameObject.tag != triggerTag) return;

        Debug.Log($"OnTriggerExit2D: {other.gameObject.name} with tag {triggerTag}");
        onTriggerExit.Invoke();
        triggered = true;
    }
}