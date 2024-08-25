using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType
    {
        Leaf,
        IceCrystal,
        LavaCrystal,
        Skull,
        Pearl,
        Bubble,
        BlackCrystal
    }

    public CollectibleType collectibleType; // Assign this in the Inspector

    private CollectibleSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<CollectibleSpawner>();
    }

    public void Collect()
    {
        // Access the sprite corresponding to the collectible type from the CollectibleSpriteManager
        Sprite collectedSprite = CollectibleSpriteManager.Instance.GetSpriteForType(collectibleType);

        if (collectedSprite != null)
        {
            // Use the collectedSprite for any purpose, such as UI display, etc.
            Debug.Log("Collected a " + collectibleType + " with sprite: " + collectedSprite.name);
        }

        // Notify the CollectibleManager that this collectible has been collected
        CollectibleManager.Instance.CollectCollectible(collectibleType);

        // Notify the spawner to spawn a new collectible
        spawner.OnCollectibleCollected();

        // Destroy the collectible
        Destroy(gameObject);
    }
}
