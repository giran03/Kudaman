using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    [Tooltip("How fast the light flickers")]
    [Range(1f, 50f)]
    public float flickerSpeed = 10f;

    [Tooltip("How much the intensity varies from the base intensity")]
    [Range(0f, 1f)]
    public float intensityVariation = 0.1f;

    [Tooltip("How much the radius varies from the base radius")]
    [Range(0f, 1f)]
    public float radiusVariation = 0.1f;

    [Tooltip("Smoothing factor for transitions (lower = smoother)")]
    [Range(1f, 50f)]
    public float smoothing = 15f;

    [Header("Color Variation")]
    [Tooltip("Enable subtle color variation")]
    public bool enableColorVariation = true;

    [Tooltip("How much the color can vary")]
    [Range(0f, 0.1f)]
    public float colorVariation = 0.05f;

    private Light2D lightComponent;
    private float baseIntensity;
    private float baseRadius;
    private Color baseColor;
    private float randomOffset;

    private float targetIntensity;
    private float targetRadius;
    private Color targetColor;

    private void Start()
    {
        lightComponent = GetComponent<Light2D>();
        baseIntensity = lightComponent.intensity;
        baseRadius = lightComponent.pointLightOuterRadius;
        baseColor = lightComponent.color;
        randomOffset = Random.value * 100f;

        // Set initial targets
        targetIntensity = baseIntensity;
        targetRadius = baseRadius;
        targetColor = baseColor;
    }

    private void Update()
    {
        // Generate noise based on time
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);
        
        // Calculate new targets
        targetIntensity = baseIntensity + (noise - 0.5f) * intensityVariation * baseIntensity;
        targetRadius = baseRadius + (noise - 0.5f) * radiusVariation * baseRadius;

        if (enableColorVariation)
        {
            float colorNoise = Mathf.PerlinNoise(Time.time * flickerSpeed * 0.5f, randomOffset + 1000f);
            targetColor = new Color(
                baseColor.r + (colorNoise - 0.5f) * colorVariation,
                baseColor.g + (colorNoise - 0.5f) * colorVariation * 0.5f, // Less variation in green
                baseColor.b - Mathf.Abs(colorNoise - 0.5f) * colorVariation * 0.2f  // Subtle blue variation
            );
        }

        // Smoothly interpolate current values to targets
        lightComponent.intensity = Mathf.Lerp(lightComponent.intensity, targetIntensity, Time.deltaTime * smoothing);
        lightComponent.pointLightOuterRadius = Mathf.Lerp(lightComponent.pointLightOuterRadius, targetRadius, Time.deltaTime * smoothing);
        
        if (enableColorVariation)
        {
            lightComponent.color = Color.Lerp(lightComponent.color, targetColor, Time.deltaTime * smoothing);
        }
    }

    // Helper method to reset the light to its base values
    public void ResetLight()
    {
        lightComponent.intensity = baseIntensity;
        lightComponent.pointLightOuterRadius = baseRadius;
        lightComponent.color = baseColor;
        targetIntensity = baseIntensity;
        targetRadius = baseRadius;
        targetColor = baseColor;
    }
}