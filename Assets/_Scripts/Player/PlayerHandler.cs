using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn;
using Yarn.Unity;

public interface IInteractable
{
    void Interact();
}

public class PlayerHandler : MonoBehaviour
{
    [Header("Configs")]

    [DisableIf(EConditionOperator.Or, "isPuteli", "isPlayer")]
    public bool isUgkuga;
    [DisableIf(EConditionOperator.Or, "isUgkuga", "isPlayer")]
    public bool isPuteli;
    [DisableIf(EConditionOperator.Or, "isPuteli", "isUgkuga")]
    public bool isPlayer;

    [ShowIf("isPlayer")]
    [DisableIf("isFemale")]
    public bool isMale;
    [ShowIf("isPlayer")]
    [DisableIf("isMale")]
    public bool isFemale;
    [SerializeField] Sprite ugkugaSprite;
    [SerializeField] Sprite puteliSprite;
    [SerializeField] Sprite playerSprite;
    [SerializeField] int _speed = 5;
    [SerializeField] float interactionRadius;
    [Space]
    // Sound _sfx_footsteps;
    // [SerializeField] Sound sfx_footsteps_region1;
    // [SerializeField] Sound sfx_footsteps_region4;

    [HideInInspector] public Vector2 _movementVector;
    [HideInInspector] public Rigidbody2D _rb;
    [HideInInspector] public Animator animator;

    public static PlayerInput _playerInput;
    public static bool IsMazeMinigameActive { get; set; }
    bool _canMove = true;
    bool isFootstepsOnCd;
    GameObject _otherGameobject;

    static PlayerEvents playerEvents;
    SpriteSwitch spriteSwitch;
    DialogueRunner dialogueRunner;
    DialogueAdvanceInput dialogueInput;
    NPC lastNPC;

    private void Awake()
    {
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        playerEvents = GetComponent<PlayerEvents>();
        spriteSwitch = GetComponent<SpriteSwitch>();

        Application.targetFrameRate = 60;

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerInput.actions.Enable();
    }

    private void OnDisable()
    {
        _playerInput.actions.Disable();
    }

    private void Start()
    {
        // _playerInput.actions["Tap Interaction"].started += ctx => Interact();

        dialogueInput = FindFirstObjectByType<DialogueAdvanceInput>();
        dialogueInput.enabled = false;

        var circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = interactionRadius;

        dialogueRunner.onNodeComplete.AddListener(delegate (string nodeName)
        {
            Debug.Log($"test {nodeName}");
        });

        if (isUgkuga)
            spriteSwitch.spriteTexture = ugkugaSprite.texture;
        else if (isPuteli)
            spriteSwitch.spriteTexture = puteliSprite.texture;
        else if (isPlayer)
            spriteSwitch.spriteTexture = playerSprite.texture;
    }

    private void Update()
    {
        // if (_movementVector.magnitude > .5f)
        // {
        //     // SFX
        //     if (!isFootstepsOnCd)
        //         StartCoroutine(PlayFootsteps());
        // }

        // Remove all player control when we're in dialogue
        if (dialogueRunner.IsDialogueRunning == true)
        {
            return;
        }

        // every time we LEAVE dialogue we have to make sure we disable the input again
        if (dialogueInput.enabled)
        {
            dialogueInput.enabled = false;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log($"Keyboard is pressed in this frame!");
            CheckForNearbyNPC();
        }
    }

    private void OnMovement(InputValue value) => _movementVector = value.Get<Vector2>();

    private void FixedUpdate()
    {
        if (_canMove)
            _rb.AddForce(_movementVector * _speed);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            lastNPC = other.GetComponent<NPC>();
            other.GetComponent<NPC>().Enable_SpriteQuestionMark();
        }

        // if (other.CompareTag("Region"))
        // {
        //     // FOOTSTEPS SFX
        //     string[] grassyRegions = { "1", "2", "3" };
        //     string[] rockyRegions = { "4", "5", "6" };

        //     if (grassyRegions.Any(name => other.name.Contains(name)))
        //         _sfx_footsteps = sfx_footsteps_region1;
        //     else if (rockyRegions.Any(name => other.name.Contains(name)))
        //         _sfx_footsteps = sfx_footsteps_region4;

        //     // BGM
        //     if (other.name.Contains("1") || other.name.Contains("2"))
        //         SingletonHandler.musicManager.PlayMusic("Region1BGM");
        //     else if (other.name.Contains("3"))
        //         SingletonHandler.musicManager.PlayMusic("Region3BGM");
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        lastNPC?.Disable_SpriteQuestionMark();
    }

    // void Interact()
    // {
    //     _otherGameobject?.GetComponent<IInteractable>().Interact(gameObject);

    //     //sfx
    //     SingletonHandler.globalSFX.PlayPopSFX();
    // }

    // calls when dialogue is initialized
    public void DisableMovement()
    {
        _canMove = false;
    }

    // calls when dialogue is finished
    public void EnableMovement()
    {
        _canMove = true;
        CameraTransition.instance.TransitionToPlayer();
        lastNPC?.Disable_QuestionMarkBubble();
        // GameUIControls.instance.ToggleInteractKeyUIButton(true);
    }

    // SFX
    // IEnumerator PlayFootsteps()
    // {
    //     isFootstepsOnCd = true;
    //     _sfx_footsteps.PlayWithRandomPitch(transform.position);
    //     yield return new WaitForSeconds(.75f);
    //     isFootstepsOnCd = false;
    // }

    // DIALOGUES
    public void CheckForNearbyNPC()
    {
        Debug.Log($"Im checking for nearby NPC!");
        var allParticipants = new List<NPC>(FindObjectsByType<NPC>(FindObjectsSortMode.None));
        var target = allParticipants.Find(delegate (NPC npc)
        {
            return string.IsNullOrEmpty(npc.talkToNode) == false && (new Vector2(npc.transform.position.x, npc.transform.position.y) -
                    new Vector2(transform.position.x, transform.position.y)).magnitude <= interactionRadius;
        });
        if (target != null)
        {
            // GameUIControls.instance.ToggleInteractKeyUIButton(false);
            // Kick off the dialogue at this node.
            Debug.Log($"dialogueRunner ({dialogueRunner})");
            dialogueRunner.StartDialogue(target.talkToNode);
            // reenabling the input on the dialogue
            dialogueInput.enabled = true;

            // Transition Camera to the target npc
            CameraTransition.instance.TransitionToTarget(target.transform);
        }
        else
            Debug.Log($"No nearby NPC found!");
    }

    [YarnCommand("startMazeMinigame")]
    public static void StartMazeMinigame()
    {
        IsMazeMinigameActive = true;
        playerEvents.OnMazeMiniGameStart?.Invoke();
    }

    [YarnCommand("endMazeMinigame")]
    public static void EndMazeMinigame()
    {
        IsMazeMinigameActive = false;
        playerEvents.OnMazeMiniGameEnd?.Invoke();
    }

    //⚠️ DEBUG ⚠️
    [Button]
    public void ToggleMazeMinigame_DEBUG() => IsMazeMinigameActive = !IsMazeMinigameActive;
}