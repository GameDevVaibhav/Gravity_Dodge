using UnityEngine;
using DG.Tweening;

public class PanelAnimation : MonoBehaviour
{
    public float animationDuration = 0.5f;  // Duration of the animation
    public float scaleFrom = 0.5f;  // Scale to start from (smaller than full size)
    public float scaleTo = 1f;  // Scale to end at (full size)
    public Ease animationEase = Ease.OutBack;  // Easing function for the animation

    private void OnEnable()
    {
        // Set the initial scale to a smaller value
        transform.localScale = Vector3.one * scaleFrom;

        // Animate the scale to full size
        transform.DOScale(scaleTo, animationDuration)
            .SetEase(animationEase);
    }
}
