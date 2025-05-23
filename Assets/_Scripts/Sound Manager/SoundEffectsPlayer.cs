using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    public static SoundEffectsPlayer SoundInstance;

    public Sound[] sfx;

    void Awake()
    {
        if (SoundInstance == null)
            SoundInstance = this;
        else if (SoundInstance != this)
            Destroy(gameObject);
    }

    public void PlayInteractSound() => sfx[0].Play(transform.position);
    public void PlayNewQuestSound() => sfx[1].Play(transform.position);
    public void PlayQuestCompletedSound() => sfx[2].Play(transform.position);
    public void PlayUIClickedSound() => sfx[3].Play(transform.position);
    public void PlayCatSound() => sfx[4].Play(transform.position);
    public void PlayDialogueTriggerSound() => sfx[5].Play(transform.position);
    public void PlayRespawnSound() => sfx[6].Play(transform.position);
    public void PlaySFX(string sfxName)
    {
        Sound sfxToPlay = System.Array.Find(sfx, s => s.Audio.name == sfxName);
        if (sfxToPlay != null)
            sfxToPlay.Play(transform.position);
    }
}
