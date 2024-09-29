using UnityEngine;

public class InspectPlanet : MonoBehaviour
{
    public GameObject planet; // Reference to the Planet GameObject
    public float rotationSpeed = 0.2f; // Speed of the rotation for dragging
    public float quickSwipeMultiplier = 10f; // Speed multiplier for quick swipes
    public float swipeThreshold = 0.1f; // Minimum time (in seconds) to consider a swipe as "quick"
    public float slowDownRate = 0.98f; // Rate at which the spin slows down

    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isDragging;
    private Vector3 currentRotationVelocity; // Current rotation velocity for continuous rotation

    void Start()
    {
        // Find the Planet GameObject by using the PlanetSwitcher script
        PlanetSwitcher planetHolder = FindObjectOfType<PlanetSwitcher>();
        if (planetHolder != null)
        {
            planet = planetHolder.gameObject;
        }
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Menu) { return; }
        HandleTouchInput();

        // Apply continuous rotation and gradually slow down
        if (currentRotationVelocity.magnitude > 0.01f)
        {
            planet.transform.Rotate(currentRotationVelocity * Time.deltaTime, Space.World);
            currentRotationVelocity *= slowDownRate;
        }
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
                    touchStartTime = Time.time;
                    isDragging = IsTouchOnPlanet(touchStartPos);
                    currentRotationVelocity = Vector3.zero; // Stop any existing spin
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        RotatePlanet(touch, false);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging)
                    {
                        RotatePlanet(touch, true);
                    }
                    isDragging = false;
                    break;
            }
        }
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

    void RotatePlanet(Touch touch, bool isEnding)
    {
        Vector2 deltaPosition = touch.deltaPosition;
        float timeElapsed = Time.time - touchStartTime;

        float speedMultiplier = rotationSpeed;
        if (isEnding)
        {
            float swipeSpeed = deltaPosition.magnitude / timeElapsed;
            if (swipeSpeed > swipeThreshold)
            {
                speedMultiplier *= quickSwipeMultiplier;
                Debug.Log("Quick");
                // Calculate the initial velocity for continuous rotation
                currentRotationVelocity = new Vector3(deltaPosition.y, -deltaPosition.x, 0f) * speedMultiplier;
                return;
            }
        }

        // Regular drag rotation
        float rotationX = deltaPosition.normalized.y * speedMultiplier*Time.deltaTime;
        float rotationY = -deltaPosition.normalized.x * speedMultiplier*Time.deltaTime;

        // Rotate the planet in world space
        planet.transform.Rotate(Vector3.right, rotationX, Space.World);
        planet.transform.Rotate(Vector3.up, rotationY, Space.World);
    }
}
