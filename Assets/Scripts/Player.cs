using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the Obstacle component
        //Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
        //if (obstacle != null)
        //{
        //    Debug.Log("Collision with obstacle");
        //}
        Collectible collectible = other.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectible.Collect();  // Collect the collectible
        }
    }
    
}
