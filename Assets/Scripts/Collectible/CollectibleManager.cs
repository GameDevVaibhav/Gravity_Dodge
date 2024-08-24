using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    private Dictionary<Collectible.CollectibleType, int> collectibleCounts = new Dictionary<Collectible.CollectibleType, int>();

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

        // Load collectible counts from JSON
        collectibleCounts = DataLoader.LoadCollectibleCounts();
        
    }

    public void CollectCollectible(Collectible.CollectibleType type)
    {
        collectibleCounts[type]++;
        Debug.Log(type + " collected. Total: " + collectibleCounts[type]);

        
    }

    public void SaveCollectibleCounts()
    {
        CollectibleData data = new CollectibleData(collectibleCounts);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/collectibleCounts.json", json);
        Debug.Log("Collectible counts saved to " + Application.persistentDataPath + "/collectibleCounts.json");
    }

}
