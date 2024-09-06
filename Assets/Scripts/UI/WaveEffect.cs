using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // Import DoTween

public class WaveEffect : MonoBehaviour
{
    public float animationDuration = 0.3f; // How long each button takes to pop out
    public float delayBetweenButtons = 0.1f; // Delay between each button popping out

    private Button[] buttons;

    private void OnEnable()
    {
        // Get all Button components in the vertical layout group
        buttons = GetComponentsInChildren<Button>();

        // Animate each button one by one
        AnimateButtons();
    }

    private void AnimateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];

            // Set initial scale to zero for the pop-out effect
            button.transform.localScale = Vector3.zero;

            // Animate scale from 0 to 1 with a delay between each button for the wave effect
            button.transform.DOScale(Vector3.one, animationDuration)
                .SetDelay(i * delayBetweenButtons) // Delay based on the button's index
                .SetEase(Ease.OutBack); // Optional: Customize the ease for a smooth pop effect
        }
    }
}
