using UnityEngine;

public class HudHandler : MonoBehaviour
{
    public void PlayButtonClickSFX() => SoundEffectsPlayer.Instance.PlaySFX("buttonClick");
}
