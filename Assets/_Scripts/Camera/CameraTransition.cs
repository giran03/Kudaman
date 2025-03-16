using System.Collections;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public static CameraTransition instance;

    [Header("References")]
    public Transform player; // Reference to the player GameObject

    [Header("Settings")]
    public float smoothSpeed = 0.125f; // Speed of the camera transition
    public Vector3 offset; // Offset of the camera from the target

    private Transform currentTarget; // Current target the camera is following

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
        // StopCoroutine(SmoothTransition(currentTarget));
        StartCoroutine(SmoothTransition(player));
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
