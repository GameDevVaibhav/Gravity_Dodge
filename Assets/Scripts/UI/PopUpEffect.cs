using UnityEngine;
using DG.Tweening;

public class PopUpEffect : MonoBehaviour
{
    public float popDuration = 0.3f;  // Duration for the pop effect
    public float popStartDelay = 0.5f;  // Delay before the pop effect starts
    public Ease popEase = Ease.OutBack;  // Easing type for the pop effect
    public Ease reverseEase = Ease.InBack;  // Easing type for the reverse pop effect
    public Vector3 startScale = Vector3.zero;  // Initial scale for the pop effect
    public Vector3 endScale = Vector3.one;  // Final scale for the pop effect
    public float reversePopDuration = 0.2f;  // Duration for the reverse pop effect

    private void OnEnable()
    {
        // Ensure the scale is initially set to the starting scale
        transform.localScale = startScale;

        // Animate the scale from startScale to endScale with DoTween, adding a delay
        transform.DOScale(endScale, popDuration)
            .SetDelay(popStartDelay)  // Add the delay before starting the pop effect
            .SetEase(popEase);  // Set the easing effect
    }

    public void PopIn()
    {
        transform.DOKill();
        // Animate the scale back to zero
        transform.DOScale(Vector3.zero, reversePopDuration)
            .SetEase(reverseEase)  // Set the easing effect for reverse animation
            .OnComplete(() => gameObject.SetActive(false));
    }
}
