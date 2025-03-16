using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    public PlayerHandler playerHandler; // Reference to the PlayerHandler script
    public Animator animator; // Reference to the Animator component
    public SpriteSwitch spriteSwitch; // Reference to the SpriteSwitch script

    [Header("Animator Controllers")]
    public RuntimeAnimatorController idleAnimatorController; // Animator controller for idle
    public RuntimeAnimatorController upAnimatorController; // Animator controller for moving up
    public RuntimeAnimatorController downAnimatorController; // Animator controller for moving down
    public RuntimeAnimatorController leftAnimatorController; // Animator controller for moving left
    public RuntimeAnimatorController rightAnimatorController; // Animator controller for moving right

    [Header("Sprite Configs")]
    [SerializeField] Sprite idleSpriteSheet;
    [SerializeField] Sprite walkSpriteSheet;

    void Update()
    {
        UpdateAnimatorController();
    }

    private void UpdateAnimatorController()
    {
        Vector2 movementVector = playerHandler._movementVector;

        if (movementVector.y > 0)
        {
            // Moving up
            animator.runtimeAnimatorController = upAnimatorController;
            spriteSwitch.spriteTexture = walkSpriteSheet.texture;
        }
        else if (movementVector.y < 0)
        {
            // Moving down
            animator.runtimeAnimatorController = downAnimatorController;
            spriteSwitch.spriteTexture = walkSpriteSheet.texture;
        }
        else if (movementVector.x < 0)
        {
            // Moving left
            animator.runtimeAnimatorController = leftAnimatorController;
            spriteSwitch.spriteTexture = walkSpriteSheet.texture;
        }
        else if (movementVector.x > 0)
        {
            // Moving right
            animator.runtimeAnimatorController = rightAnimatorController;
            spriteSwitch.spriteTexture = walkSpriteSheet.texture;
        }
        else
        {
            // Idle
            animator.runtimeAnimatorController = idleAnimatorController;
            spriteSwitch.spriteTexture = idleSpriteSheet.texture;
        }
    }
}