using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;

    [Header("Audio Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float defaultMusicVolume = 0.5f;
    [SerializeField] private float defaultSFXVolume = 0.7f;
    [SerializeField] private float defaultAmbientVolume = 0.4f;

    private AudioClip currentMusicClip;
    private AudioClip currentAmbientClip;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Create audio source if not assigned
            // Initialize audio sources if not assigned
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.volume = defaultMusicVolume;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.loop = false;
                sfxSource.volume = defaultSFXVolume;
            }

            if (ambientSource == null)
            {
                ambientSource = gameObject.AddComponent<AudioSource>();
                ambientSource.loop = true;
                ambientSource.volume = defaultAmbientVolume;
            }

            // Subscribe to scene loading events
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene events
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Load and play the appropriate music for the new scene
        string musicPath = $"Music/{scene.name}BGM";
        AudioClip newMusic = Resources.Load<AudioClip>(musicPath);

        if (newMusic != null && newMusic != currentMusicClip)
        {
            PlayMusic(newMusic);
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Optional: Fade out music when scene is unloaded
        if (musicSource.isPlaying)
        {
            StartFade(0f);
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null) return;

        currentMusicClip = musicClip;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // If music is already playing, fade between tracks
        if (musicSource.isPlaying)
        {
            fadeCoroutine = StartCoroutine(CrossFadeMusic(musicClip));
        }
        else
        {
            musicSource.clip = musicClip;
            musicSource.volume = defaultMusicVolume;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip == null) return;
        sfxSource.PlayOneShot(sfxClip, defaultSFXVolume);
    }

    public void PlayAmbient(AudioClip ambientClip)
    {
        if (ambientClip == null) return;

        currentAmbientClip = ambientClip;
        ambientSource.clip = ambientClip;
        ambientSource.volume = defaultAmbientVolume;
        ambientSource.Play();
    }

    public void StopMusic()
    {
        StartFade(0f);
    }

    private void StartFade(float targetVolume)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeMusic(targetVolume));
    }

    private IEnumerator CrossFadeMusic(AudioClip newClip)
    {
        float timeElapsed = 0;
        float startVolume = musicSource.volume;

        // Fade out current music
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / fadeDuration);
            yield return null;
        }

        // Switch to new music
        musicSource.clip = newClip;
        musicSource.Play();
        timeElapsed = 0;

        // Fade in new music
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, defaultMusicVolume, timeElapsed / fadeDuration);
            yield return null;
        }

        musicSource.volume = defaultMusicVolume;
        fadeCoroutine = null;
    }

    private IEnumerator FadeMusic(float targetVolume)
    {
        float timeElapsed = 0;
        float startVolume = musicSource.volume;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / fadeDuration);
            yield return null;
        }

        musicSource.volume = targetVolume;

        if (targetVolume <= 0)
        {
            musicSource.Stop();
        }

        fadeCoroutine = null;
    }
}