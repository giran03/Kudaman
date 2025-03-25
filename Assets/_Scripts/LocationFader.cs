using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

[RequireComponent(typeof(CanvasGroup))]
public class LocationFader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField, Range(0f, 1f)] private float minAlpha = 0f;
    [SerializeField, Range(0f, 1f)] private float maxAlpha = 1f;
    [SerializeField] private bool startFadingOnAwake = true;

    [Header("Location Settings")]
    [SerializeField] private List<Transform> locationPoints = new List<Transform>();
    [SerializeField] private float moveSpeed = 5f;

    private CanvasGroup canvasGroup;
    private int currentLocationIndex = 0;
    private bool isFading = false;
    private bool isMoving = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (startFadingOnAwake)
        {
            StartFading();
        }
    }

    public void StartFading()
    {
        if (!isFading)
        {
            StartCoroutine(FadeRoutine());
        }
    }

    public void StopFading()
    {
        isFading = false;
        StopAllCoroutines();
    }

    private IEnumerator FadeRoutine()
    {
        isFading = true;
        
        while (isFading)
        {
            // Fade Out
            yield return StartCoroutine(FadeToTarget(minAlpha));
            
            // Fade In
            yield return StartCoroutine(FadeToTarget(maxAlpha));
        }
    }

    private IEnumerator FadeToTarget(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    [Button("Test Next Location")]
    public void NextLocation()
    {
        if (locationPoints.Count == 0) return;

        currentLocationIndex = (currentLocationIndex + 1) % locationPoints.Count;
        if (locationPoints[currentLocationIndex] != null)
        {
            StartCoroutine(MoveToLocation(locationPoints[currentLocationIndex].position));
        }
    }

    public void SetLocation(int index)
    {
        if (index >= 0 && index < locationPoints.Count && locationPoints[index] != null)
        {
            currentLocationIndex = index;
            StartCoroutine(MoveToLocation(locationPoints[currentLocationIndex].position));
        }
    }

    private IEnumerator MoveToLocation(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (transform.position != targetPosition)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }

        isMoving = false;
    }

    public void AddLocation(Transform location)
    {
        if (location != null && !locationPoints.Contains(location))
        {
            locationPoints.Add(location);
        }
    }

    public void RemoveLocation(Transform location)
    {
        locationPoints.Remove(location);
    }

    public void ClearLocations()
    {
        locationPoints.Clear();
        currentLocationIndex = 0;
    }
}