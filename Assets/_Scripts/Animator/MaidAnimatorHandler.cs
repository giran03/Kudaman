using NaughtyAttributes;
using UnityEngine;

public class MaidAnimatorHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] RuntimeAnimatorController animatorControllerIdle;
    [SerializeField] SpriteSwitch spriteSwitch;
    [SerializeField] Sprite spriteTextureWalk;
    [SerializeField] Sprite spriteTextureIdle;

    [Button]
    public void ChangeAnimatorController()
    {
        spriteSwitch.spriteTexture = spriteTextureWalk.texture;
        animator.runtimeAnimatorController = animatorController;
    }

    [Button]
    public void ChangeToIdle()
    {
        spriteSwitch.spriteTexture = spriteTextureIdle.texture;
        animator.runtimeAnimatorController = animatorControllerIdle;
    }
}
