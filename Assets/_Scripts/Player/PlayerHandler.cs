using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Yarn;
using Yarn.Unity;

public interface IInteractable
{
    void Interact();
}

public class PlayerHandler : MonoBehaviour
{
    [Header("Configs")]
    public bool transitionAfterDialogue = true;
    public bool TransitionToPlayer2 { get; set; }
    [Space]
    [Header("Sprite Configs")]
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

    [Header("Gender Sprite Variants")]
    [ShowIf("isMale")]
    public Sprite[] male_2_SpriteVariants;
    [ShowIf("isMale")]
    public Sprite[] male_3_SpriteVariants;
    [Space]
    [ShowIf("isFemale")]
    public Sprite[] female_2_SpriteVariants;
    [ShowIf("isFemale")]
    public Sprite[] female_3_SpriteVariants;

    [Space]
    [SerializeField] Sprite ugkugaSprite;
    [SerializeField] Sprite puteliSprite;
    [SerializeField] Sprite playerSprite;
    [SerializeField] int _speed = 5;
    [SerializeField] float interactionRadius;
    [Space]

    [HideInInspector] public Vector2 _movementVector;
    [HideInInspector] public Rigidbody2D _rb;
    [HideInInspector] public Animator animator;

    public static PlayerInput _playerInput;
    public static bool IsMazeMinigameActive { get; set; }
    bool _canMove = true;

    public static PlayerEvents playerEvents;
    SpriteSwitch spriteSwitch;
    DialogueRunner dialogueRunner;
    DialogueAdvanceInput dialogueInput;
    NPC lastNPC;
    public PlayerAudio playerAudio;

    // auto move
    float timeElapsed = 0;

    string selectedGender;

    //minigames
    MazeMinigame mazeMinigame;

    private void Awake()
    {
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        playerEvents = GetComponent<PlayerEvents>();
        spriteSwitch = GetComponent<SpriteSwitch>();

        Application.targetFrameRate = 60;

        animator = GetComponent<Animator>();

        mazeMinigame = FindFirstObjectByType<MazeMinigame>();

        //get player gender
        if (isPlayer)
        {
            var playerGenderFromPrefs = PlayerPrefs.GetString("playerGender");
            selectedGender = playerGenderFromPrefs;
            if (playerGenderFromPrefs == "male")
            {
                isMale = true;
                isFemale = false;
            }
            else if (playerGenderFromPrefs == "female")
            {
                isMale = false;
                isFemale = true;
            }
            else
            {
                Debug.LogWarning($"No player gender found in PlayerPrefs. Defaulting to Male.");
                isMale = true;
                isFemale = false;
            }
        }
    }

    private void OnEnable()
    {
        _playerInput?.actions.Enable();
    }

    private void OnDisable()
    {
        _playerInput?.actions.Disable();
    }

    private void Start()
    {
        // _playerInput.actions["Tap Interaction"].started += ctx => Interact();
        playerAudio = GetComponent<PlayerAudio>();

        dialogueInput = FindFirstObjectByType<DialogueAdvanceInput>();
        dialogueInput.enabled = false;

        var circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = interactionRadius;

        // dialogueRunner.onNodeComplete.AddListener(delegate (string nodeName)
        // {
        // });

        if (isUgkuga)
            spriteSwitch.spriteTexture = ugkugaSprite.texture;
        else if (isPuteli)
            spriteSwitch.spriteTexture = puteliSprite.texture;
        else if (isPlayer)
            spriteSwitch.spriteTexture = playerSprite.texture;
    }

    private void Update()
    {

        // Remove all player control when we're in dialogue
        if (dialogueRunner.IsDialogueRunning == true)
            return;

        // every time we LEAVE dialogue we have to make sure we disable the input again
        if (dialogueInput.enabled)
            dialogueInput.enabled = false;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            CheckForNearbyNPC();
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

        if (mazeMinigame != null)
        {
            Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
            Vector3Int cellPosSign = mazeMinigame.tilemapPushable.WorldToCell(mazeMinigame.playerOrigin.transform.position);

            foreach (Vector3Int direction in directions)
            {
                Vector3Int targetPos = cellPosSign + direction;
                if (other.gameObject.TryGetComponent<Tilemap>(out var tileMap))
                {
                    tileMap.GetTile(targetPos);

                    TileBase targetPushableTile = mazeMinigame.tilemapPushable.GetTile(targetPos);

                    if (targetPushableTile == mazeMinigame.pushableTile)
                    {
                        mazeMinigame.pushText.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        lastNPC?.Disable_SpriteQuestionMark();

        if (mazeMinigame != null)
            if (mazeMinigame.tilemapPushable != other.gameObject.GetComponent<Tilemap>())
                if (mazeMinigame.pushText != null)
                    mazeMinigame.pushText.SetActive(false);
    }

    // calls when dialogue is initialized
    public void DisableMovement()
    {
        _canMove = false;
        SoundEffectsPlayer.SoundInstance.PlayNewQuestSound();
    }

    // calls when dialogue is finished
    public void EnableMovement()
    {
        _canMove = true;
        if (transitionAfterDialogue)
            CameraTransition.instance.TransitionToPlayer();
        lastNPC?.Disable_QuestionMarkBubble();
        SoundEffectsPlayer.SoundInstance.PlayQuestCompletedSound();
        // GameUIControls.instance.ToggleInteractKeyUIButton(true);
    }

    // DIALOGUES
    public void CheckForNearbyNPC()
    {
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
            dialogueRunner.StartDialogue(target.talkToNode);
            // reenabling the input on the dialogue
            dialogueInput.enabled = true;

            // Transition Camera to the target npc
            CameraTransition.instance.TransitionToTarget(target.transform);

            SoundEffectsPlayer.SoundInstance.PlayDialogueTriggerSound();
        }
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

    public void EnablePlayerInput() => _playerInput.enabled = true;

    public void DisableTransitionToSelf() => transitionAfterDialogue = false;

    public void EnableTransitionToSelf() => transitionAfterDialogue = true;

    // auto move
    [Button]
    [YarnCommand("MoveLeft_Puteli")]
    public void MoveLeft() => StartCoroutine(MoveUpWithDuration(2f));

    public IEnumerator MoveUpWithDuration(float duration)
    {
        while (timeElapsed < duration)
        {
            _movementVector = Vector2.left;
            _rb.AddForce(_movementVector * _speed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _movementVector = Vector2.zero;
    }

    [Button]
    public void CheckDialogueSpriteToUse() => Debug.Log($"DialogueSpriteToUse: {DialogueSpriteToUse()}");
    public string DialogueSpriteToUse()
    {
        var selectedGenderVariant = PlayerPrefs.GetInt("playerGenderVariant");
        var selectedGender = PlayerPrefs.GetString("playerGender");
        if (selectedGenderVariant == 0)
        {
            if (selectedGender == "male")
                return "male_1";
            else
                return "female_1";
        }
        else if (selectedGenderVariant == 1)
        {
            if (selectedGender == "male")
                return "male_2";
            else
                return "female_2";
        }
        else if (selectedGenderVariant == 2)
        {
            if (selectedGender == "male")
                return "male_3";
            else
                return "female_3";
        }
        return "???";
    }

    public string GetSelectedGender() => selectedGender = PlayerPrefs.GetString("playerGender");

    //⚠️ DEBUG ⚠️
    [Button]
    public void ToggleMazeMinigame_DEBUG() => IsMazeMinigameActive = !IsMazeMinigameActive;

    [Button]
    public void LogTransitionPlayer2() => Debug.Log($"TransitionToPlayer2: {TransitionToPlayer2}");

    [Button]
    public void LogTransitionAfterDialogue() => Debug.Log($"transitionAfterDialogue: {transitionAfterDialogue}");
}