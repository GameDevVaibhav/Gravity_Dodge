using UnityEngine;
using System.Collections;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject collectiblePrefab;  // Prefab of the collectible to spawn (Orb)
    public GameObject speedupPrefab;      // Prefab of the Speedup collectible
    public Transform planetCenter;        // Reference to the planet's center
    public float spawnRadius = 5f;        // Radius at which collectibles spawn from the center
    public int initialCollectibleCount = 3; // Number of collectibles to spawn initially
    public int maxCollectibles = 3;       // Maximum number of Orb collectibles to have at any time

    private int currentCollectibleCount = 0;
    private bool isSpeedupActive = false; // Track if Speedup is currently active

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
        // Spawn the initial set of Orbs
        for (int i = 0; i < initialCollectibleCount; i++)
        {
            SpawnCollectible();
        }

        // Spawn the initial Speedup collectible
        SpawnSpeedup();
    }

    // Method to spawn an Orb at a random location on the planet's surface
    public void SpawnCollectible()
    {
        if (currentCollectibleCount >= maxCollectibles)
            return;

        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 spawnPosition = planetCenter.position + randomDirection * spawnRadius;

        GameObject newCollectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
        newCollectible.transform.SetParent(transform);

        currentCollectibleCount++;
    }

    // Method to spawn a Speedup collectible at a random location on the planet's surface
    public void SpawnSpeedup()
    {
        if (isSpeedupActive)
            return;

        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 spawnPosition = planetCenter.position + randomDirection * spawnRadius;

        GameObject newSpeedup = Instantiate(speedupPrefab, spawnPosition, Quaternion.identity);
        newSpeedup.transform.SetParent(transform);

        isSpeedupActive = true;
    }

    // Method to handle when a Speedup is collected
    public void OnSpeedupCollected()
    {
        isSpeedupActive = false;
        StartCoroutine(RespawnSpeedupAfterDelay(10f)); // Wait 10 seconds before respawning
    }
    public void OnCollectibleCollected()
    {
        currentCollectibleCount--;

        // Spawn a new collectible to maintain the count
        SpawnCollectible();
    }

    private IEnumerator RespawnSpeedupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnSpeedup();
    }

    public void ClearAllCollectibles()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        currentCollectibleCount = 0;
        isSpeedupActive = false; // Reset the Speedup collectible state
    }
}
