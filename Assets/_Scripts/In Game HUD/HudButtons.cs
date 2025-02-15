using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudButtons : MonoBehaviour
{
    [SerializeField] Button menuPauseButton;
    [SerializeField] Button menuResumeButton;

    [Header("Pop Up Animation")]
    [SerializeField] RectTransform popUpMenuRect;
    [SerializeField] float targetPosX;
    [SerializeField] float tweenDuration;

    [Header("Configs")]
    [SerializeField] CanvasGroup darkScreenPauseBG;
    [SerializeField] CanvasGroup controlsCanvasGroup;

    bool isPaused = false;
    float initialPosX;

    void Start()
    {
        menuPauseButton?.onClick.AddListener(Pause);
        menuResumeButton?.onClick.AddListener(ResumeAsync);

        initialPosX = popUpMenuRect.position.x;
        popUpMenuRect.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Toggle pause with ESC key
        {
            if (isPaused)
                ResumeAsync();
            else
                Pause();
        }
    }

    void Pause()
    {
        popUpMenuRect.gameObject.SetActive(true);
        PopUpMenuIntro();
        isPaused = true;
        Debug.Log($"Game is paused!");
        Time.timeScale = 0f; // Stop time
    }

    public async void ResumeAsync()
    {
        await PopUpMenuOutro();
        isPaused = false;
        Debug.Log($"Game is not paused!");
        Time.timeScale = 1f; // Resume time
        popUpMenuRect.gameObject.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Reset time before loading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Pop Up Animations
    void PopUpMenuIntro()
    {
        darkScreenPauseBG.DOFade(1, tweenDuration).SetUpdate(true);
        controlsCanvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        popUpMenuRect.DOAnchorPosX(targetPosX, tweenDuration).SetUpdate(true);
    }

    async Task PopUpMenuOutro()
    {
        darkScreenPauseBG.DOFade(0, tweenDuration).SetUpdate(true);
        controlsCanvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        await popUpMenuRect.DOAnchorPosX(initialPosX, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
}