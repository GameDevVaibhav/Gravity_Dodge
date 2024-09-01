using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectVehicle : MonoBehaviour
{
    public Transform targetObject; // The object to be rotated
    public RawImage rawImage; // The RawImage on which the RenderTexture is displayed
    public float rotationSpeed = 0.5f; // Speed of rotation

    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        // Check if the pointer is over the RawImage
        if (IsPointerOverUIElement())
        {
            if (Input.GetMouseButtonDown(0)) // When we start dragging
            {
                lastMousePosition = Input.mousePosition;
                isDragging = true;
            }

            if (Input.GetMouseButton(0) && isDragging) // While dragging
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;

                // Calculate the rotation angles based on the swipe movement
                float rotationX = -delta.y * rotationSpeed; // Vertical movement rotates around the X axis
                float rotationY = delta.x * rotationSpeed; // Horizontal movement rotates around the Y axis

                // Apply the rotation to the target object
                targetObject.Rotate(Vector3.up, rotationY, Space.World);
                targetObject.Rotate(Vector3.right, rotationX, Space.World);

                // Update the last mouse position
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0)) // When we stop dragging
            {
                isDragging = false;
            }
        }
    }

    // This function checks if the mouse pointer is over the RawImage UI element
    private bool IsPointerOverUIElement()
    {
        if (rawImage == null)
            return false;

        RectTransform rectTransform = rawImage.rectTransform;
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);

        return rectTransform.rect.Contains(localMousePosition);
    }
}
