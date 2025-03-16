using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimationController : MonoBehaviour
{
    [Header("References")]
    public Image targetImage; // Reference to the Image UI component

    [Header("Sprites")]
    public List<Sprite> sprites; // List of sprites for the animation

    [Header("Settings")]
    public float frameDuration = 0.1f; // Duration of each frame in the animation

    private int currentFrame = 0; // Index of the current frame in the animation
    private Coroutine animationCoroutine; // Reference to the running animation coroutine

    void Start()
    {
        // Start the sprite animation
        StartAnimation();
        PulseScale();
    }

    /// <summary>
    /// Starts the sprite animation.
    /// </summary>
    public void StartAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateSprites());
    }

    /// <summary>
    /// Stops the sprite animation.
    /// </summary>
    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    /// <summary>
    /// Coroutine to animate the sprites.
    /// </summary>
    private IEnumerator AnimateSprites()
    {
        while (true)
        {
            // Set the current sprite
            targetImage.sprite = sprites[currentFrame];

            // Wait for the specified frame duration
            yield return new WaitForSeconds(frameDuration);

            // Move to the next frame
            currentFrame = (currentFrame + 1) % sprites.Count;
        }
    }

    /// <summary>
    /// Pulsates the image's scale.
    /// </summary>
    public void PulseScale()
    {
        targetImage.rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
}