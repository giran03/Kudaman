using UnityEngine;
using Yarn.Unity;

public class DisableAfterTrigger_Puteli_2 : MonoBehaviour
{
    [YarnCommand("Disable_Puteli_2")]
    public void DisableAfterTrigger()
    {
        gameObject.SetActive(false);
        FindFirstObjectByType<PlayerEvents>().OnPuteliDialogue_2_end?.Invoke();
    }
}
