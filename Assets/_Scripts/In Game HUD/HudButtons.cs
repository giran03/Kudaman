using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudButtons : MonoBehaviour
{
    [SerializeField] Button menuPauseButton;
    [SerializeField] Button homeButton;
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
        homeButton?.onClick.AddListener(GoToMainMenu);
        menuPauseButton?.onClick.AddListener(Pause);
        menuResumeButton?.onClick.AddListener(ResumeAsync);

        initialPosX = popUpMenuRect.position.x;
        // popUpMenuRect.gameObject.SetActive(false);
    }

    void Pause()
    {
        // popUpMenuRect.gameObject.SetActive(true);
        PopUpMenuIntro();
        isPaused = true;
        Debug.Log($"Game is paused!");
        Time.timeScale = 0f; // Stop time
    }

    public async void ResumeAsync()
    {
        await PopUpMenuOutro();
        Time.timeScale = 1f; // Resume time
        isPaused = false;
        Debug.Log($"Game is not paused!");
        // popUpMenuRect.gameObject.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Reset time before loading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reset time before loading
        SceneManager.LoadScene("MainMenu");
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