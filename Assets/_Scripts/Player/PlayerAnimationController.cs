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
    [SerializeField] Sprite ugkuga_idleSpriteSheet;
    [SerializeField] Sprite ugkuga_walkSpriteSheet;
    [Space]
    [SerializeField] Sprite puteli_idleSpriteSheet;
    [SerializeField] Sprite puteli_walkSpriteSheet;
    [Space]
    [SerializeField] Sprite player_male_idleSpriteSheet;
    [SerializeField] Sprite player_male_walkSpriteSheet;
    [Space]
    [SerializeField] Sprite player_female_idleSpriteSheet;
    [SerializeField] Sprite player_female_walkSpriteSheet;

    Sprite active_idleSpriteSheet;
    Sprite active_walkSpriteSheet;

    private void Start()
    {
        var playerHandler = GetComponent<PlayerHandler>();

        if (playerHandler.isUgkuga)
        {
            active_idleSpriteSheet = ugkuga_idleSpriteSheet;
            active_walkSpriteSheet = ugkuga_walkSpriteSheet;
        }
        else if (playerHandler.isPuteli)
        {
            active_idleSpriteSheet = puteli_idleSpriteSheet;
            active_walkSpriteSheet = puteli_walkSpriteSheet;
        }
        else if (playerHandler.isPlayer)
        {
            if (playerHandler.isMale)
            {
                active_idleSpriteSheet = player_male_idleSpriteSheet;
                active_walkSpriteSheet = player_male_walkSpriteSheet;
            }
            else if (playerHandler.isFemale)
            {
                active_idleSpriteSheet = player_female_idleSpriteSheet;
                active_walkSpriteSheet = player_female_walkSpriteSheet;
            }
        }
    }

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
            spriteSwitch.spriteTexture = active_walkSpriteSheet.texture;
        }
        else if (movementVector.y < 0)
        {
            // Moving down
            animator.runtimeAnimatorController = downAnimatorController;
            spriteSwitch.spriteTexture = active_walkSpriteSheet.texture;
        }
        else if (movementVector.x < 0)
        {
            // Moving left
            animator.runtimeAnimatorController = leftAnimatorController;
            spriteSwitch.spriteTexture = active_walkSpriteSheet.texture;
        }
        else if (movementVector.x > 0)
        {
            // Moving right
            animator.runtimeAnimatorController = rightAnimatorController;
            spriteSwitch.spriteTexture = active_walkSpriteSheet.texture;
        }
        else
        {
            // Idle
            animator.runtimeAnimatorController = idleAnimatorController;
            spriteSwitch.spriteTexture = active_idleSpriteSheet.texture;
        }
    }
}