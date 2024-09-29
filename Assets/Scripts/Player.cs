using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public PlanetRotation planetRotation;
    public bool isInvincible;

    public GameObject explosionVFX;
    public float slowMotionFactor = 0.2f; // Factor by which to slow down time
    public float delayBeforeGameOver = 3f; // Delay before game over, in seconds (real-time)

    Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (planetRotation == null)
        {
            planetRotation = FindObjectOfType<PlanetRotation>();
        }

        // Check if the collided object has the Obstacle component
        Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            if (!isInvincible)
            {
                Debug.Log("Collision with obstacle");

                // Define the rotation offset (180 degrees around the Y-axis)
                Quaternion rotationOffset = Quaternion.Euler(0, 180, 0);

                // Instantiate explosion VFX at the obstacle's position with the rotation offset and set it as a child of the PlanetRotation object
                GameObject vfx = Instantiate(explosionVFX, transform.position, rotationOffset);
                vfx.transform.SetParent(planetRotation.transform);

                transform.GetChild(0).gameObject.SetActive(false);
                
                collider.enabled=false;

                // Start the coroutine to handle slow motion and then game over
                StartCoroutine(HandleCollision());
            }
        }

        Collectible collectible = other.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectible.Collect();  // Collect the collectible
        }

        Speedup speedup = other.gameObject.GetComponent<Speedup>();
        if (speedup != null)
        {
            speedup.Collect();  // Collect the Speedup
        }
    }

    // Coroutine to handle slow motion and then game over
    private IEnumerator HandleCollision()
    {
       

        // Slow down time
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f; // Adjust physics to match the slowed time

        // Wait for the specified delay in real-time
        yield return new WaitForSecondsRealtime(delayBeforeGameOver);

        // Reset time scale to normal
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // Reset physics time step

        // Trigger Game Over state after the delay
        GameManager.Instance.UpdateGameState(GameState.GameOver);
    }

    public void SetInvincible(bool isInvincible)
    {
        this.isInvincible = isInvincible;
    }

    

    public void ActivatePlayer()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        collider.enabled = true;
    }
}
