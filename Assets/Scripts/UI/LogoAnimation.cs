using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    [Tooltip("The RectTransform of the UI Image to move.")]
    public RectTransform uiElement;

    [Tooltip("The list of waypoints for the movement (Transforms).")]
    public List<Transform> pathTransforms;

    [Tooltip("Duration of the movement in seconds.")]
    public float duration = 5f;

    [Tooltip("Should the movement loop?")]
    public bool loopMovement = true;

    [Tooltip("The type of loop (restart, yoyo, etc.)")]
    public LoopType loopType = LoopType.Restart;

    [Tooltip("The easing function for the movement.")]
    public Ease movementEase = Ease.Linear;

    private bool hasRotated = false;  // To track if the rotation has happened
    private float rotationDuration;

    public GameObject logo;
    public float fadeInDuration = 2f;

    private void Start()
    {
        // Calculate rotation duration based on total movement duration
        rotationDuration = duration; // Full rotation occurs over the course of the movement

        // Start moving the UI element along the path
        MoveAlongThePath();
    }

    private void MoveAlongThePath()
    {
        // Convert the pathTransforms into an array of positions
        Vector3[] pathPoints = new Vector3[pathTransforms.Count];
        for (int i = 0; i < pathTransforms.Count; i++)
        {
            pathPoints[i] = pathTransforms[i].position;
        }

        // Move the UI element along the given path
        uiElement.DOPath(pathPoints, duration, PathType.CatmullRom)
            .SetEase(movementEase)
            .SetLoops(loopMovement ? -1 : 0, loopType)
            .OnWaypointChange(OnWaypointReached)
            .OnComplete(ActivateObject);   // Update rotation when reaching new waypoint
    }

    private void OnWaypointReached(int waypointIndex)
    {
        // Ensure we haven't already finished the rotation
        if (hasRotated) return;

        // Ensure the waypoint index is valid
        if (waypointIndex < 0 || waypointIndex >= pathTransforms.Count - 1)
        {
            return;
        }

        // If this is the first waypoint, start the rotation from -50
        if (waypointIndex == 0)
        {
            // Start rotating the UI element from -50 degrees to 360 degrees
            uiElement.DORotate(new Vector3(0, 0, 360), rotationDuration, RotateMode.FastBeyond360)
                .From(new Vector3(0, 0, -50))
                .SetEase(Ease.Linear)
                .OnComplete(() => hasRotated = true);  // Mark rotation as complete
        }
    }

    private void ActivateObject()
    {
        if (logo != null)
        {
            // Activate the object
            logo.SetActive(true);

            // Check if the object has a CanvasGroup (required for fading)
            CanvasGroup canvasGroup = logo.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                // If there's no CanvasGroup, add one
                canvasGroup = logo.AddComponent<CanvasGroup>();
            }

            // Set the initial alpha to 0 (completely transparent)
            canvasGroup.alpha = 0f;

            // Fade in by increasing the alpha from 0 to 1 over the specified duration
            canvasGroup.DOFade(1f, fadeInDuration).SetEase(Ease.InOutQuad);
        }
        else
        {
            Debug.LogWarning("No object assigned to activate after the animation.");
        }
    }
}

