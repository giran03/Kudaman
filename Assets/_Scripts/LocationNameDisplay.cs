using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LocationNameDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI locationText;
    [SerializeField] private Image locationBackground;

    [Header("Display Settings")]
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private string locationName = "Location Name";

    private void Start()
    {
        // Ensure the text and background are initially invisible
        if (locationText != null && locationBackground != null)
        {
            locationText.alpha = 0f;
            locationBackground.color = new Color(locationBackground.color.r, locationBackground.color.g, locationBackground.color.b, 0f);
            StartCoroutine(ShowLocationName());
        }
        else
        {
            Debug.LogWarning("LocationNameDisplay: Text or background component not assigned!");
        }
    }

    private IEnumerator ShowLocationName()
    {
        // Fade in
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            locationText.alpha = alpha;
            locationBackground.color = new Color(locationBackground.color.r, locationBackground.color.g, locationBackground.color.b, alpha);
            yield return null;
        }
        locationText.alpha = 1f;
        locationBackground.color = new Color(locationBackground.color.r, locationBackground.color.g, locationBackground.color.b, 1f);

        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            locationText.alpha = alpha;
            locationBackground.color = new Color(locationBackground.color.r, locationBackground.color.g, locationBackground.color.b, alpha);
            yield return null;
        }
        locationText.alpha = 0f;
        locationBackground.color = new Color(locationBackground.color.r, locationBackground.color.g, locationBackground.color.b, 0f);
    }

    // Optional: Public method to manually trigger the location name display
    public void DisplayLocation(string newLocationName)
    {
        locationName = newLocationName;
        locationText.text = locationName;
        StartCoroutine(ShowLocationName());
    }
}