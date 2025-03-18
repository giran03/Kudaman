using System.Collections.Generic;
using NaughtyAttributes;
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
    [SerializeField] int _speed = 5;
    [SerializeField] float interactionRadius = 2;
    [Space]
    // Sound _sfx_footsteps;
    // [SerializeField] Sound sfx_footsteps_region1;
    // [SerializeField] Sound sfx_footsteps_region4;

    [HideInInspector] public Vector2 _movementVector;
    [HideInInspector] public Rigidbody2D _rb;
    [HideInInspector] public Animator animator;

    [Header("Dialogue")]
    public DialogueRunner dialogueRunner;

    public static PlayerInput _playerInput;
    bool _canMove = true;
    bool isFootstepsOnCd;
    GameObject _otherGameobject;

    DialogueAdvanceInput dialogueInput;
    NPC lastNPC;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();

        Application.targetFrameRate = 60;

        animator = GetComponent<Animator>();
    }

    private void OnEnable() => _playerInput.actions.Enable();

    private void OnDisable() => _playerInput.actions.Disable();

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

    private void OnTriggerEnter2D(Collider2D other)
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
        var allParticipants = new List<NPC>(FindObjectsByType<NPC>(FindObjectsSortMode.None));
        var target = allParticipants.Find(delegate (NPC npc)
        {
            return string.IsNullOrEmpty(npc.talkToNode) == false && (npc.transform.position - transform.position).magnitude <= interactionRadius;
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
        }
    }
}