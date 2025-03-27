using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

// ADD THIS TO EACH GAMEOBJECT THAT WILL BE SET TO TRIGGER AUTOMATIC CAMERA SEQUENCES
public class AutoCameraSequenceTrigger : MonoBehaviour
{
    [Header("Configs")]
    [Tooltip("Add the functions to be executed after the camera sequence ends.")]
    public List<Action> onEndMethods = new();
    [Space]
    [Tooltip("Add the index of the camera sequence to trigger from 'CameraSequenceHandler' game object.")]
    [SerializeField] int sequenceIndexToTrigger;
    [Space]
    [Tooltip("Add the node to start the dialogue from the 'Yarn' script.")]
    [SerializeField] string nodeToStart;
    BoxCollider2D boxCollider2D;

    DialogueRunner dialogueRunner;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = true;

        dialogueRunner = FindFirstObjectByType<DialogueRunner>();


        if (nodeToStart != null || nodeToStart != "")
            onEndMethods.Add(() => dialogueRunner.StartDialogue(nodeToStart));

        CameraSequenceHandler.Instance.onCameraSequenceEnd.AddListener(ExecuteFunctions);
    }

    void OnDisable()
    {
        CameraSequenceHandler.Instance.onCameraSequenceEnd.RemoveListener(ExecuteFunctions);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //disable player movement
            collision.gameObject.GetComponent<PlayerHandler>().DisableMovement();
            Debug.Log($"Player has entered the auto camera trigger!");

            //add the function to enable player movement to the list of functions to be executed
            if (nodeToStart == null || nodeToStart == "")
                onEndMethods.Add(collision.gameObject.GetComponent<PlayerHandler>().EnableMovement);

            StartCoroutine(CameraSequenceHandler.Instance.AutoCameraSequence(sequenceIndexToTrigger));
            boxCollider2D.enabled = false;
        }
    }

    void ExecuteFunctions() => onEndMethods?.ForEach(f => f());
}