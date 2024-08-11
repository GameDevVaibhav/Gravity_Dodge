using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    private int collectibleCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist through scene loads
        }
        else
        {
            Destroy(gameObject);  // Ensure only one instance exists
        }
    }

    public void CollectCollectible()
    {
        collectibleCount++;
        Debug.Log("Collectibles collected: " + collectibleCount);
        // You can also update UI or trigger other game events here
    }
}
