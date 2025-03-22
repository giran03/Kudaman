using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingManager : MonoBehaviour
{
    private int currentPresetIndex = 0;
    [UnityEngine.Serialization.FormerlySerializedAs("NextPreset")]
    private bool _nextPreset;

    [NaughtyAttributes.Button("Next Preset")]
    private void NextPreset()
    {
        GoToNextPreset();
    }

    [System.Serializable]
    public class DayLightPreset
    {
        public string timeName;
        public string description;
        public Color ambientColor;
        public Color globalLightColor;
        public float intensity;
        [Range(0f, 10f)]
        public float lightRadius = 5f;
        [Range(0f, 1f)]
        public float shadowIntensity = 0.5f;
        [Range(1000f, 10000f)]
        public float colorTemperature = 6500f;
    }

    [Header("Light Settings")]
    public Light2D globalLight;
    [SerializeField]
    private DayLightPreset[] dayLightPresets;
    [SerializeField]
    private string defaultPresetName;

    private void OnValidate()
    {
        if (dayLightPresets == null || dayLightPresets.Length == 0)
        {
            dayLightPresets = new DayLightPreset[]
            {
                new DayLightPreset
                {
                    timeName = "Morning",
                    description = "The royal garden, under the shade of towering trees. Morning and bright",
                    ambientColor = new Color(0.9f, 0.9f, 0.95f),
                    globalLightColor = new Color(1f, 0.98f, 0.9f),
                    intensity = 0.85f,
                    lightRadius = 8f,
                    shadowIntensity = 0.3f,
                    colorTemperature = 6000f
                },
                new DayLightPreset
                {
                    timeName = "Afternoon",
                    description = "A university courtyard, late afternoon. The sky is tinged with hues of orange and pink",
                    ambientColor = new Color(1f, 0.9f, 0.8f),
                    globalLightColor = new Color(1f, 0.85f, 0.7f),
                    intensity = 0.9f,
                    lightRadius = 7f,
                    shadowIntensity = 0.4f,
                    colorTemperature = 5000f
                },
                new DayLightPreset
                {
                    timeName = "Late Afternoon",
                    description = "Late afternoon, deep in the forest. Visuals will have a warm tone indicating the setting sun",
                    ambientColor = new Color(0.95f, 0.8f, 0.6f),
                    globalLightColor = new Color(1f, 0.75f, 0.5f),
                    intensity = 0.75f,
                    lightRadius = 6f,
                    shadowIntensity = 0.6f,
                    colorTemperature = 4000f
                },
                new DayLightPreset
                {
                    timeName = "Evening",
                    description = "The mysterious Balete Tree, environment dark blue from the moonlight, surrounded by flickering fireflies",
                    ambientColor = new Color(0.3f, 0.4f, 0.6f),
                    globalLightColor = new Color(0.4f, 0.5f, 0.7f),
                    intensity = 0.5f,
                    lightRadius = 4f,
                    shadowIntensity = 0.7f,
                    colorTemperature = 3000f
                },
                new DayLightPreset
                {
                    timeName = "Late Evening",
                    description = "Dark blue hue within the royal palace to indicate the night sky",
                    ambientColor = new Color(0.2f, 0.25f, 0.4f),
                    globalLightColor = new Color(0.3f, 0.35f, 0.5f),
                    intensity = 0.3f,
                    lightRadius = 3f,
                    shadowIntensity = 0.8f,
                    colorTemperature = 2000f
                }
            };
        }
    }
    public float transitionDuration = 2f;

    private DayLightPreset currentPreset;
    private DayLightPreset targetPreset;
    private float transitionTimer;
    private bool isTransitioning;

    private void Start()
    {
        // Set initial lighting to the default preset if specified, otherwise use the first preset
        if (dayLightPresets != null && dayLightPresets.Length > 0)
        {
            if (!string.IsNullOrEmpty(defaultPresetName))
            {
                TransitionToPreset(defaultPresetName);
            }
            else
            {
                SetLightingPreset(dayLightPresets[0]);
            }
        }
    }

    private void Update()
    {
        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            // Smoothly interpolate between current and target lighting settings
            Color currentAmbient = Color.Lerp(currentPreset.ambientColor, targetPreset.ambientColor, t);
            Color currentGlobalLight = Color.Lerp(currentPreset.globalLightColor, targetPreset.globalLightColor, t);
            float currentIntensity = Mathf.Lerp(currentPreset.intensity, targetPreset.intensity, t);
            float currentRadius = Mathf.Lerp(currentPreset.lightRadius, targetPreset.lightRadius, t);
            float currentShadowIntensity = Mathf.Lerp(currentPreset.shadowIntensity, targetPreset.shadowIntensity, t);
            float currentColorTemp = Mathf.Lerp(currentPreset.colorTemperature, targetPreset.colorTemperature, t);

            // Apply the interpolated values
            RenderSettings.ambientLight = currentAmbient;
            globalLight.color = currentGlobalLight;
            globalLight.intensity = currentIntensity;
            globalLight.pointLightOuterRadius = currentRadius;
            globalLight.shadowIntensity = currentShadowIntensity;
            // Convert color temperature to RGB (simplified approximation)
            float temp = currentColorTemp / 100f;
            float red = temp <= 66 ? 255 : Mathf.Clamp(329.698727446f * Mathf.Pow(temp - 60, -0.1332047592f), 0, 255);
            float green = temp <= 66 ? Mathf.Clamp(99.4708025861f * Mathf.Log(temp) - 161.1195681661f, 0, 255) : Mathf.Clamp(288.1221695283f * Mathf.Pow(temp - 60, -0.0755148492f), 0, 255);
            float blue = temp >= 66 ? 255 : temp <= 19 ? 0 : Mathf.Clamp(138.5177312231f * Mathf.Log(temp - 10) - 305.0447927307f, 0, 255);
            globalLight.color *= new Color(red/255f, green/255f, blue/255f, 1f);

            if (t >= 1)
            {
                isTransitioning = false;
                currentPreset = targetPreset;
            }
        }
    }

    public void TransitionToPreset(string presetName)
    {
        DayLightPreset preset = System.Array.Find(dayLightPresets, x => x.timeName == presetName);
        if (preset != null)
        {
            currentPresetIndex = System.Array.FindIndex(dayLightPresets, x => x.timeName == presetName);
            StartTransition(preset);
        }
        else
        {
            Debug.LogWarning($"Preset {presetName} not found!");
        }
    }

    private void GoToNextPreset()
    {
        if (dayLightPresets == null || dayLightPresets.Length == 0) return;

        currentPresetIndex = (currentPresetIndex + 1) % dayLightPresets.Length;
        StartTransition(dayLightPresets[currentPresetIndex]);
    }

    private void StartTransition(DayLightPreset newPreset)
    {
        if (currentPreset == null)
        {
            SetLightingPreset(newPreset);
            return;
        }

        targetPreset = newPreset;
        transitionTimer = 0f;
        isTransitioning = true;
    }

    private void SetLightingPreset(DayLightPreset preset)
    {
        currentPreset = preset;
        RenderSettings.ambientLight = preset.ambientColor;
        globalLight.color = preset.globalLightColor;
        globalLight.intensity = preset.intensity;
    }
}