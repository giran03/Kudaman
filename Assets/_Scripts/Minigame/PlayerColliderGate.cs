using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderGate : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] List<GameObject> shortcutGate;
    PlayerHandler playerHandler;

    private void Awake()
    {
        playerHandler = FindFirstObjectByType<PlayerHandler>();
    }

    private void Start()
    {
        shortcutGate.ForEach(gate => gate.SetActive(false));
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger has the player tag
        if (collision.CompareTag("Player"))
        {
            if (playerHandler.isPuteli)
                shortcutGate.ForEach(gate => gate.SetActive(true));
            else
                shortcutGate.ForEach(gate => gate.SetActive(false));
        }
    }
}