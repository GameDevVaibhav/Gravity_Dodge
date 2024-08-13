using UnityEngine;

public class Collectible : MonoBehaviour
{
    private CollectibleSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<CollectibleSpawner>();
    }

    public void Collect()
    {
        // Notify the CollectibleManager that this collectible has been collected
        CollectibleManager.Instance.CollectCollectible();

        // Notify the spawner to spawn a new collectible
        spawner.OnCollectibleCollected();

        // Destroy the collectible
        Destroy(gameObject);
    }
}
