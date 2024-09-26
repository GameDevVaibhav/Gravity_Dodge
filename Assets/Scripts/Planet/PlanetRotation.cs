using System.Collections;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Rotation speed
    public float speedupMultiplier = 10f;
    public Transform objectOnPlanet;  // Reference to the object on the planet

    public Player player;


    private Vector3 initialRotationDirection = new Vector3(1, 0, 0).normalized; // Initial rotation direction
    private Vector3 currentRotationDirection = Vector3.zero;    // Current rotation direction
    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;
    private bool isTouching = false;

    private void Start()
    {
        // Set an initial rotation direction so the planet starts rotating immediately
        currentRotationDirection = initialRotationDirection;

        player=FindObjectOfType<Player>();
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Play)
        {
            ResetPlayerRotation();
            return;
        }
            


        HandleTouchInput();

        // Apply continuous rotation based on the current rotation direction
        if (currentRotationDirection != Vector3.zero)
        {
            transform.Rotate(-currentRotationDirection, rotationSpeed * Time.deltaTime, Space.World);
        }

        // Ensure the object stays on the surface of the planet
        PositionObjectOnSurface();
        RotatePlayer();
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

                    if (delta != Vector2.zero)
                    {
                        // Only update the rotation direction if there's a non-zero delta
                        currentRotationDirection = new Vector3(delta.y, -delta.x, 0).normalized;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
                // Don't reset the rotation direction here, just the touch positions
                touchStartPos = touchCurrentPos;
            }
        }
        
    }

    void PositionObjectOnSurface()
    {
        //// Calculate the direction from the planet center to the object
        //Vector3 direction = (objectOnPlanet.position - transform.position).normalized;

        //// Calculate the correct position on the surface of the planet
        //float planetRadius = transform.localScale.x / 2;
        //float objectRadius = objectOnPlanet.localScale.y / 2; // Assuming the object is a cube, change as needed
        //objectOnPlanet.position = transform.position + direction * (planetRadius + objectRadius);
        
    }
    void RotatePlayer()
    {
        // Calculate the direction opposite to the planet's rotation
        Vector3 oppositeRotationDirection = currentRotationDirection;

        // Convert the rotation direction to an angle
        float angle = Mathf.Atan2(oppositeRotationDirection.y, oppositeRotationDirection.x) * Mathf.Rad2Deg;

        // Apply the rotation to the player's z axis
        player.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // New method to reset player rotation
    public void ResetPlayerRotation()
    {
        // Reset the player's rotation to the default orientation (no rotation)
        player.transform.rotation = Quaternion.identity;
    }

    public void ResetRotationDirection()
    {
        currentRotationDirection = initialRotationDirection;
        ResetPlanetRotation();
    }

    public void SpeedUp()
    {
        StartCoroutine(SpeedUpRoutine());
    }

    private IEnumerator SpeedUpRoutine()
    {
        float originalSpeed = rotationSpeed;
        player.SetInvincible(true); // Enable invincibility
        rotationSpeed *= speedupMultiplier;

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Revert to the original rotation speed and disable invincibility
        rotationSpeed = originalSpeed;
        player.SetInvincible(false);
    }

    void ResetPlanetRotation()
    {
        gameObject.transform.rotation = Quaternion.identity;
    }

}
