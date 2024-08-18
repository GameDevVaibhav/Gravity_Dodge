using UnityEngine;

public class Speedup : MonoBehaviour
{
    private CollectibleSpawner spawner;
    private PlanetRotation planet;

    private void Start()
    {
        spawner = FindObjectOfType<CollectibleSpawner>();
        planet = FindObjectOfType<PlanetRotation>();
    }

    public void Collect()
    {
        // Trigger the SpeedUp effect on the planet's rotation
        planet.SpeedUp();

        // Notify the spawner to handle Speedup respawn
        spawner.OnSpeedupCollected();

        // Destroy the Speedup collectible
        Destroy(gameObject);
    }
}
