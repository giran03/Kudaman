using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderGate : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] List<GameObject> shortcutGate;

    private void Start()
    {
        shortcutGate.ForEach(gate => gate.SetActive(false));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerHandler>().isPuteli)
                shortcutGate.ForEach(gate => gate.SetActive(true));
            else
                shortcutGate.ForEach(gate => gate.SetActive(false));
        }
    }
}