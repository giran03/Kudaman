using System.Collections;
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
    private bool isFootstepsOnCd;

    private void Start()
    {
        var playerHandler = GetComponent<PlayerHandler>();
        var playerSelectedVariant = PlayerPrefs.GetInt("playerGenderVariant");

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
                //change sprite based on sprite variant picked
                if (playerSelectedVariant == 1)
                {
                    active_idleSpriteSheet = playerHandler.male_2_SpriteVariants[0];
                    active_walkSpriteSheet = playerHandler.male_2_SpriteVariants[1];
                }
                else if (playerSelectedVariant == 2)
                {
                    active_idleSpriteSheet = playerHandler.male_3_SpriteVariants[0];
                    active_walkSpriteSheet = playerHandler.male_3_SpriteVariants[1];
                }
                else
                {
                    active_idleSpriteSheet = player_male_idleSpriteSheet;
                    active_walkSpriteSheet = player_male_walkSpriteSheet;
                }
            }
            else if (playerHandler.isFemale)
            {
                //change sprite based on sprite variant picked
                if (playerSelectedVariant == 1)
                {
                    active_idleSpriteSheet = playerHandler.female_2_SpriteVariants[0];
                    active_walkSpriteSheet = playerHandler.female_2_SpriteVariants[1];
                }
                else if (playerSelectedVariant == 2)
                {
                    active_idleSpriteSheet = playerHandler.female_3_SpriteVariants[0];
                    active_walkSpriteSheet = playerHandler.female_3_SpriteVariants[1];
                }
                else
                {
                    active_idleSpriteSheet = player_female_idleSpriteSheet;
                    active_walkSpriteSheet = player_female_walkSpriteSheet;
                }
            }
            Debug.Log($"selected variant is {playerSelectedVariant}");
        }
    }

    void Update()
    {
        UpdateAnimatorController();
        if (playerHandler._movementVector.magnitude > .5f)
        {
            // SFX
            if (!isFootstepsOnCd)
                StartCoroutine(PlayFootsteps());
        }
    }

    private void UpdateAnimatorController()
    {
        Vector2 movementVector = playerHandler._movementVector;

        Debug.Log($"movementVector: {movementVector}");

        if (movementVector.y > 0.5f)
        {
            // Moving up
            animator.runtimeAnimatorController = upAnimatorController;
            spriteSwitch.spriteTexture = active_walkSpriteSheet.texture;
        }
        else if (movementVector.y < -0.5f)
        {
            // Moving down
            animator.runtimeAnimatorController = downAnimatorController;
            spriteSwitch.spriteTexture = active_walkSpriteSheet.texture;
        }
        else if (movementVector.x < -0.5f)
        {
            // Moving left
            animator.runtimeAnimatorController = leftAnimatorController;
            spriteSwitch.spriteTexture = active_walkSpriteSheet.texture;
        }
        else if (movementVector.x > 0.5f)
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

    IEnumerator PlayFootsteps()
    {
        isFootstepsOnCd = true;
        playerHandler.playerAudio.player_footstepsSFX?.PlayWithRandomPitch(transform.position);
        yield return new WaitForSeconds(.5f);
        isFootstepsOnCd = false;
    }
}