using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Check if the collided object has the Obstacle component
       Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            //Debug.Log("Collision with obstacle");

            //// Change game state to GameOver
            //GameManager.Instance.UpdateGameState(GameState.GameOver);
        }
        Collectible collectible = other.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectible.Collect();  // Collect the collectible
        }
    }
    
}
