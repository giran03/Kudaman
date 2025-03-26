using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class ChaseMinigame : MonoBehaviour
{
    public static ChaseMinigame Instance;

    [Header("References")]
    public GameObject player; // Reference to the player GameObject
    public GameObject[] maids; // Array of maid GameObjects
    public BoxCollider2D winZone; // BoxCollider2D for the win condition

    [Header("Settings")]
    public float chaseDuration = 30f; // Duration of the chase in seconds
    public float maidSpeed = 3f; // Speed of the maids

    [Header("Events")]
    public UnityEvent onMinigameStart; // Event triggered when the minigame starts
    public UnityEvent onPlayerLose; // Event triggered when the player loses
    public UnityEvent onMinigameEnd; // Event triggered when the minigame ends

    private bool isMinigameActive = false; // Tracks if the minigame is active
    private float chaseTimer = 0f; // Timer for the chase duration

    private Vector3 initialPlayerPosition; // Stores the initial position of the player
    private Vector3[] initialMaidPositions; // Stores the initial positions of the maids

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        // Ensure the winZone has a trigger
        if (winZone != null)
        {
            winZone.isTrigger = true;
        }

        // Save the initial positions of the player and maids
        SaveInitialPositions();
    }

    private void Update()
    {
        if (isMinigameActive)
        {
            // Update the chase timer
            chaseTimer -= Time.deltaTime;

            // Move the maids toward the player
            foreach (GameObject maid in maids)
            {
                if (maid != null)
                {
                    Vector3 direction = (player.transform.position - maid.transform.position).normalized;
                    maid.transform.position += direction * maidSpeed * Time.deltaTime;
                }
            }

            // Check if the chase duration has ended
            if (chaseTimer <= 0f)
            {
                EndMinigame(false); // Player wins if time runs out
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player enters the win zone
        if (isMinigameActive && collision.gameObject == player)
        {
            EndMinigame(true); // Player wins
        }
    }

    [Button]
    [YarnCommand("StartChaseMinigame")]
    public void StartMinigame()
    {
        if (isMinigameActive) return;

        // Initialize the minigame
        isMinigameActive = true;
        chaseTimer = chaseDuration;

        // Trigger the start event
        onMinigameStart?.Invoke();

        Debug.Log("Chase Minigame Started!");
    }

    public void EndMinigame(bool playerWon)
    {
        if (!isMinigameActive) return;

        // End the minigame
        isMinigameActive = false;

        // Reset positions if the player loses
        if (!playerWon)
        {
            // Trigger the player lose event
            onPlayerLose?.Invoke();

            ResetPositions();
        }

        // Trigger the end event
        onMinigameEnd?.Invoke();

        if (playerWon)
        {
            Debug.Log("Player Wins the Chase Minigame!");
        }
        else
        {
            Debug.Log("Player Loses the Chase Minigame!");
        }
    }

    /// <summary>
    /// Saves the initial positions of the player and the maids.
    /// </summary>
    private void SaveInitialPositions()
    {
        // Save the player's initial position
        initialPlayerPosition = player.transform.position;

        // Save the maids' initial positions
        initialMaidPositions = new Vector3[maids.Length];
        for (int i = 0; i < maids.Length; i++)
        {
            if (maids[i] != null)
            {
                initialMaidPositions[i] = maids[i].transform.position;
            }
        }

        Debug.Log("Initial positions saved.");
    }

    /// <summary>
    /// Resets the positions of the player and the maids to their initial positions.
    /// </summary>
    private void ResetPositions()
    {
        // Reset the player's position
        player.transform.position = initialPlayerPosition;

        // Reset the maids' positions
        for (int i = 0; i < maids.Length; i++)
        {
            if (maids[i] != null)
            {
                maids[i].transform.position = initialMaidPositions[i];
            }
        }

        Debug.Log("Positions reset to initial values.");
    }

    [Button]
    public void RestartMinigame()
    {
        // Reset the positions of the player and maids
        ResetPositions();

        // Reinitialize the minigame
        isMinigameActive = true;
        chaseTimer = chaseDuration;

        // Trigger the start event
        onMinigameStart?.Invoke();

        Debug.Log("Chase Minigame Restarted!");
    }
}