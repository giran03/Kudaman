using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CameraTransition : MonoBehaviour
{
    public static CameraTransition instance;

    [Header("References")]
    public Transform player; // Reference to the player GameObject
    public Transform player2;

    [Header("Settings")]
    public float smoothSpeed = 0.125f; // Speed of the camera transition
    public Vector3 offset; // Offset of the camera from the target

    private Transform currentTarget; // Current target the camera is following
    public UnityEvent OnPlayerChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        currentTarget = player; // Initialize the current target to the player
    }

    void LateUpdate()
    {
        if (currentTarget != null)
        {
            Vector3 desiredPosition = currentTarget.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }

    // Function to transition to a specified target
    public void TransitionToTarget(Transform newTarget)
    {
        Debug.Log($"Transitioning to {newTarget.name}");
        // StopCoroutine(SmoothTransition(currentTarget));
        StartCoroutine(SmoothTransition(newTarget));
    }

    // Function to transition back to the player
    public void TransitionToPlayer()
    {
        var playerHandler = FindFirstObjectByType<PlayerHandler>();
        if (playerHandler.TransitionToPlayer2 && player2 != null)
            StartCoroutine(SmoothTransition(player2));
        else
        {
            StartCoroutine(SmoothTransition(player));
            player.gameObject.GetComponent<PlayerInput>().enabled = true;
        }
    }

    [Button]
    // Function to transition to player2
    public void TransitionToPlayer2()
    {
        currentTarget = player2;
        OnPlayerChanged?.Invoke();
    }

    // Coroutine to smoothly transition between targets
    private IEnumerator SmoothTransition(Transform newTarget)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;
        Vector3 targetPosition = newTarget.position + offset;

        while (elapsedTime < smoothSpeed)
        {
            Vector3 smoothedPosition = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentTarget = newTarget;
    }
}
