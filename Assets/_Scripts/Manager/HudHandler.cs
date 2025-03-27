using UnityEngine;

public class HudHandler : MonoBehaviour
{
    public void PlayButtonClickSFX() => SoundEffectsPlayer.SoundInstance.PlaySFX("buttonClick");
}
