using UnityEngine;
using Yarn.Unity;

public class DisableAfterTrigger_Puteli_1 : MonoBehaviour
{
    [YarnCommand("Disable_Puteli_1")]
    public void DisableAfterTrigger() => gameObject.SetActive(false);
}