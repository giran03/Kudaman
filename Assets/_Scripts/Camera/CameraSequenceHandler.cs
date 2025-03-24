using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class CameraSequenceHandler : MonoBehaviour
{
    public static CameraSequenceHandler instance;

    [Header("References")]
    public List<CameraSequenceTarget> cameraSequenceTargets = new(); // List of all camera sequence targets

    [Header("Settings")]
    public float transitionDuration = 2.0f; // Duration the camera stays on each target before transitioning

    [Header("Events")]
    public UnityEvent onCameraSequenceEnd;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    /// <summary> Use this function to automatically trigger a camera sequence. </summary>
    public IEnumerator AutoCameraSequence(int sequenceNumber)
    {
        if (sequenceNumber < 0 || sequenceNumber >= cameraSequenceTargets.Count)
        {
            Debug.LogWarning("Sequence number out of bounds, No sequence in this index");
            // yield break;
        }
        else
        {
            CameraSequenceTarget target = cameraSequenceTargets[sequenceNumber];
            // Transition to the current target
            foreach (var gameObjectTransforms in target.cameraTargets)
            {
                CameraTransition.instance.TransitionToTarget(gameObjectTransforms);
                // Wait for the specified duration before transitioning to the next target
                yield return new WaitForSeconds(transitionDuration);
            }
        }

        // Transition back to the player after the sequence is complete
        CameraTransition.instance.TransitionToPlayer();

        // Invoke the onCameraSequenceEnd event
        onCameraSequenceEnd?.Invoke();
    }

    /// <summary> Use this function to manually trigger a camera sequence. </summary>
    [YarnCommand("ManualCameraSequence")]
    public void ManualCameraSequence(int sequenceNumber, int targetIndex)
    {
        if (sequenceNumber < 0 || sequenceNumber >= cameraSequenceTargets.Count)
        {
            Debug.LogError("Target index out of bounds");
            return;
        }

        CameraSequenceTarget target = cameraSequenceTargets[sequenceNumber];
        // Transition to the current target
        CameraTransition.instance.TransitionToTarget(target.cameraTargets[targetIndex]);
    }
}