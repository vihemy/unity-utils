using System.Collections;
using UnityEngine;

public class ScaleAnimator : MonoBehaviour
{
    public float scaleMultiplier = 1.2f;
    public float speed = 1.0f;
    public float pauseDuration = 1.0f; // Duration of the pause after the scale returns to original
    public bool animateOnEnable = true; // Should the animation start when the object is enabled?

    private RectTransform rectTransform;
    private Vector3 minSize;
    private Vector3 maxSize => minSize * scaleMultiplier;
    private Coroutine currentAnimation;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        minSize = rectTransform.localScale;
    }

    void OnEnable()
    {
        if (animateOnEnable)
        {
            StartAnimation();
        }
    }

    public void StartAnimation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(AnimateScale());
    }

    public void StopAnimation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }
        rectTransform.localScale = minSize;
    }

    private IEnumerator AnimateScale()
    {
        while (true)
        {
            // Animate from minSize to maxSize with ease-out
            yield return StartCoroutine(ScaleOverTime(minSize, maxSize, speed, false));

            // Animate back from maxSize to minSize with ease-in
            yield return StartCoroutine(ScaleOverTime(maxSize, minSize, speed, true));

            // Pause
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float duration, bool easeIn)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Apply ease-in or ease-out curve
            t = easeIn ? Mathf.Sin(t * Mathf.PI * 0.5f) // Ease-in with sine function
                       : 1f - Mathf.Cos(t * Mathf.PI * 0.5f); // Ease-out with cosine function

            rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = endScale;
    }
}
