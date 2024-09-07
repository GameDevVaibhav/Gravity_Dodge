using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SwipeButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform parentContainer;  // The parent container (UI element) to stretch
    public float stretchDuration = 0.3f;  // Duration for the stretch effect
    public float squashFactor = 0.8f;    // Factor to squash the container
    public float stretchFactor = 1.2f;    // Factor to stretch the container
    public int bounceCount = 3;          // Number of bounce iterations

    private Vector2 originalSize; // To store the original size of the container
    private float dragEndPositionX; // To store the position of X when drag ends

    private void Start()
    {
        // Store the original size of the container
        originalSize = parentContainer.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position = transform.localPosition;
        float newX = position.x + eventData.delta.x;

        // Clamp the position of the button itself
        transform.localPosition = new Vector3(Mathf.Clamp(newX, -79, 79), position.y, position.z);

        // Store the X position when dragging ends
        dragEndPositionX = newX;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 position = transform.localPosition;

        if (position.x < 79 && position.x >= -79)
        {
            transform.localPosition = new Vector3(-79, position.y, position.z);
        }
        else
        {
            GameManager.Instance.OnPlayButtonClicked();
            transform.localPosition = new Vector3(-79, position.y, position.z);
        }

        // Check if the user tried to drag beyond the limit and apply stretch and squash effect
        if (dragEndPositionX > 79 || dragEndPositionX < -79)
        {
            ApplyStretchAndSquash();
        }
    }

    private void ApplyStretchAndSquash()
    {
       

        // Start the bounce sequence
        Sequence bounceSequence = DOTween.Sequence();
        

        // Repeat the bounce sequence with decreasing strength
        for (int i = 1; i < bounceCount; i++)
        {
            float strengthMultiplier = Mathf.Pow(0.5f, i); // Decrease strength gradually
            bounceSequence.Append(parentContainer.DOSizeDelta(new Vector2(originalSize.x * (1 + strengthMultiplier), originalSize.y * (1 - strengthMultiplier)), stretchDuration / 2)
                .SetEase(Ease.OutQuad))
                .Append(parentContainer.DOSizeDelta(new Vector2(originalSize.x * (1 - strengthMultiplier), originalSize.y * (1 + strengthMultiplier)), stretchDuration / 2)
                .SetEase(Ease.InQuad));
        }

        // Return to original size at the end
        bounceSequence.Append(parentContainer.DOSizeDelta(originalSize, stretchDuration / 2)
            .SetEase(Ease.OutQuad));
    }
}
