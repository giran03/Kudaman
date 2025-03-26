using UnityEngine;
using Yarn.Unity;

public class DisableAfterTrigger_Puteli_2 : MonoBehaviour
{
    [YarnCommand("Disable_Puteli_2")]
    public void DisableAfterTrigger()
    {
        var puteli = GameObject.Find("Player_1").GetComponent<PlayerEvents>();
        puteli.OnPuteliDialogue_2_end?.Invoke();
        Debug.Log($"puteli_2 {puteli.gameObject.name}");
        gameObject.SetActive(false);
    }
}
