using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SwipeButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform parentContainer;  // The parent container (UI element) to move
    public float bounceDuration = 0.5f;    // Duration of the bounce effect
    public float elasticity = 0.5f;        // Elasticity factor for the bounce effect (higher = more elastic)

    private float dragEndPositionX;         // To store the position of X when drag ends

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
            BounceBackToLeftWall();
        }
        else
        {
            GameManager.Instance.OnPlayButtonClicked();
            BounceBackToLeftWall();
        }
    }

    private void BounceBackToLeftWall()
    {
        // Kill any existing animations on the button
        transform.DOKill();

        // Apply elastic bounce-back effect
        transform.DOLocalMoveX(-79, bounceDuration)
            .SetEase(Ease.OutBounce, elasticity)  // Use OutElastic easing for bounce
            .OnComplete(() => transform.localPosition = new Vector3(-79, transform.localPosition.y, transform.localPosition.z));
    }
}
