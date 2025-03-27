using UnityEngine;
using Yarn.Unity;

public class TriggerUgkugaEyes : MonoBehaviour
{
    public GameObject ugkugaEyes;

    private void Start() => ugkugaEyes.SetActive(false);

    [YarnCommand("UgkugaEyesOpen")]
    public void UgkugaEyesOpen() => ugkugaEyes.SetActive(true);

    [YarnCommand("UgkugaEyesClose")]
    public void UgkugaEyesClose() => ugkugaEyes.SetActive(false);
}
