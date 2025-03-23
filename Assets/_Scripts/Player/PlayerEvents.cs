using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    [Header("Player Events")]
    public UnityEvent OnMazeMiniGameStart;
    public UnityEvent OnMazeMiniGameEnd;

    // ⚠️ DEBUG ⚠️
    public void TestLog()
    {
        Debug.Log("Test Log");
    }
}
