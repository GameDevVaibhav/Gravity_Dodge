using UnityEngine;

public class InspectPlanet : MonoBehaviour
{
    public GameObject planet; // Reference to the Planet GameObject
    public float rotationSpeed = 5f; // Speed of the rotation

    private Vector2 touchStartPos;
    private bool isDragging;

    private Vector3 currentRotation; // Stores the current rotation of the planet

    void Start()
    {
        // Find the Planet GameObject by using the PlanetSwitcher script
        PlanetSwitcher planetHolder = FindObjectOfType<PlanetSwitcher>();
        if (planetHolder != null)
        {
            planet = planetHolder.gameObject;
            currentRotation = planet.transform.eulerAngles;
        }
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Menu) { return; }
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isDragging = IsTouchOnPlanet(touchStartPos);
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        RotatePlanet(touch);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }

        // Smoothly interpolate the rotation
        planet.transform.rotation = Quaternion.Lerp(planet.transform.rotation, Quaternion.Euler(-currentRotation), Time.deltaTime * rotationSpeed);
    }

    bool IsTouchOnPlanet(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // Check if the touch began on the planet
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Planet");
            return hit.collider.GetComponent<PlanetRotation>() != null;
        }

        return false;
    }

    void RotatePlanet(Touch touch)
    {
        Vector2 deltaPosition = touch.deltaPosition;

        // Adjust rotation based on touch movement
        currentRotation.y += deltaPosition.x * rotationSpeed; // Rotate around the Y axis
        currentRotation.x -= deltaPosition.y * rotationSpeed; // Rotate around the X axis
    }
}
