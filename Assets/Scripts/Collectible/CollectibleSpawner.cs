using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject collectiblePrefab;  // Prefab of the collectible to spawn
    public Transform planetCenter;        // Reference to the planet's center
    public float spawnRadius = 5f;        // Radius at which collectibles spawn from the center
    public int initialCollectibleCount = 3; // Number of collectibles to spawn initially
    public int maxCollectibles = 3;       // Maximum number of collectibles to have at any time

    private int currentCollectibleCount = 0;
    void OnEnable()
    {
        GameManager.OnGameRestart += ClearAllCollectibles;
    }

    void OnDisable()
    {
        GameManager.OnGameRestart -= ClearAllCollectibles;
    }
    public void InitialSpawn()
    {
        // Spawn the initial set of collectibles
        for (int i = 0; i < initialCollectibleCount; i++)
        {
            SpawnCollectible();
        }
    }

    // Method to spawn a collectible at a random location on the planet's surface
    public void SpawnCollectible()
    {
        
        if (currentCollectibleCount >= maxCollectibles)
            return;

        // Generate a random position on the surface of the planet
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 spawnPosition = planetCenter.position + randomDirection * spawnRadius;

        // Instantiate the collectible and set it as a child of the spawner
        GameObject newCollectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
        newCollectible.transform.SetParent(transform);

        currentCollectibleCount++;
    }

    // Method to decrease the count of active collectibles when one is collected
    public void OnCollectibleCollected()
    {
        currentCollectibleCount--;

        // Spawn a new collectible to maintain the count
        SpawnCollectible();
    }

    public void ClearAllCollectibles()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        currentCollectibleCount = 0;  // Reset the collectible count
       // InitialSpawn();
    }
}
