using UnityEngine;
using Yarn.Unity;

public class AnimationTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject targetGameObject; // The GameObject with the Animator component
    public string animationTriggerName; // The name of the animation trigger

    public Animator animator; // Reference to the Animator component

    [YarnCommand("TriggerAnimation")]
    public void TriggerAnimation(string animationTriggerName)
    {
        if (this.animationTriggerName == animationTriggerName)
        {
            // Enable the target GameObject
            targetGameObject.SetActive(true);

            // Trigger the animation
            animator.SetTrigger(animationTriggerName);

            Debug.Log($"Triggered animation: {animationTriggerName}");
        }
    }

    [YarnCommand("DisableAfterAnimation")]
    public void DisableAfterAnimation(string animationTriggerName)
    {
        if (this.animationTriggerName == animationTriggerName)
            StartCoroutine(WaitForAnimationToEnd());
    }

    private System.Collections.IEnumerator WaitForAnimationToEnd()
    {
        // Wait until the current animation is done playing
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || animator.IsInTransition(0))
        {
            yield return null;
        }

        // Disable the target GameObject
        targetGameObject.SetActive(false);

        Debug.Log("Animation finished. GameObject disabled.");
    }

    [YarnCommand("PlayAnimationDialogue")]
    public void PlayAnimationDialogue(string animationTriggerName)
    {
        this.animationTriggerName = animationTriggerName;
        TriggerAnimation(this.animationTriggerName);
    }
}
