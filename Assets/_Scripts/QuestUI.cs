using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip questUpdateSFX;
    private static QuestUI _instance;

    public static QuestUI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QuestUI>();
            }
            return _instance;
        }
    }

    public TextMeshProUGUI questText;
    public Image backgroundImage;
    public GameObject questPanel;
    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UpdateQuestText(QuestSO quest)
    {
        if (audioSource != null && questUpdateSFX != null)
        {
            audioSource.PlayOneShot(questUpdateSFX);
        }
        questPanel.SetActive(true);
        questText.text = quest.description;
        RectTransform panelRect = questPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(600, 300);
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    public void StartFade()
    {
        StartCoroutine(FadeInOut());
    }

    private System.Collections.IEnumerator FadeInOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}