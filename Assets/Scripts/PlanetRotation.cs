using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Rotation speed
    public Transform objectOnPlanet;  // Reference to the object on the planet

    private Vector3 currentRotationDirection = Vector3.zero;  // Current rotation direction
    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;
    private bool isTouching = false;

    void Update()
    {
        HandleTouchInput();

        // Apply continuous rotation based on the current rotation direction
        if (currentRotationDirection != Vector3.zero)
        {
            transform.Rotate(-currentRotationDirection, rotationSpeed * Time.deltaTime, Space.World);
        }

        // Ensure the object stays on the surface of the planet
        PositionObjectOnSurface();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isTouching = true;
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (isTouching)
                {
                    touchCurrentPos = touch.position;
                    Vector2 delta = touchCurrentPos - touchStartPos;
                    currentRotationDirection = new Vector3(delta.y,- delta.x, 0 ).normalized;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
                touchStartPos = touchCurrentPos;
            }
        }
    }

    void PositionObjectOnSurface()
    {
        // Calculate the direction from the planet center to the object
        Vector3 direction = (objectOnPlanet.position - transform.position).normalized;

        // Calculate the correct position on the surface of the planet
        float planetRadius = transform.localScale.x / 2;
        float objectRadius = objectOnPlanet.localScale.y / 2; // Assuming the object is a cube, change as needed
        objectOnPlanet.position = transform.position + direction * (planetRadius + objectRadius);
    }
}
