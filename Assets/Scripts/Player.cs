using UnityEngine;

public class Player : MonoBehaviour
{
    public PlanetRotation planetRotation;
    private bool isInvincible;

    private void OnTriggerEnter(Collider other)
    {
        if (planetRotation == null)
        {
            planetRotation = FindObjectOfType<PlanetRotation>();
        }
        
        //Check if the collided object has the Obstacle component
        Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            if (!isInvincible)
            {
                Debug.Log("Collision with obstacle");
                GameManager.Instance.UpdateGameState(GameState.GameOver);
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

    public void SetInvincible(bool isInvincible)
    {
        this.isInvincible = isInvincible;
    }
    
}
