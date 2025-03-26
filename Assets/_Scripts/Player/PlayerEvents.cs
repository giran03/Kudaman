using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class PlayerEvents : MonoBehaviour
{
    [Header("Player Events")]
    public UnityEvent OnMazeMiniGameStart;
    public UnityEvent OnMazeMiniGameEnd;
    public UnityEvent OnBaleteInteractStart;
    public UnityEvent OnBaleteInteractEnd;
    public UnityEvent OnPlayerChanged;
    public UnityEvent OnPuteliBaleteInteractStart;
    public UnityEvent OnPuteliDialogue_2_end;
    public UnityEvent OnUgkugaTriggerEnding;

    [YarnCommand("OnBaleteInteractStart")]
    public void InteractBalete() => OnBaleteInteractStart?.Invoke();

    [YarnCommand("OnBaleteInteractEnd")]
    public void StopInteractBalete() => OnBaleteInteractEnd?.Invoke();

    [YarnCommand("OnPuteliBaleteInteractStart")]
    public void PuteliBaleteInteractStart() => OnPuteliBaleteInteractStart?.Invoke();

    [YarnCommand("OnUgkugaTriggerEnding")]
    public void UgkugaTriggerEnding() => OnUgkugaTriggerEnding?.Invoke();

    // ⚠️ DEBUG ⚠️
    public void TestLog() => Debug.Log("Test Log");
}
