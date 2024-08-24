using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLoader 
{
    
    
    public static Dictionary<Collectible.CollectibleType, int> LoadCollectibleCounts()
    {
         string filePath = Application.persistentDataPath + "/collectibleCounts.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CollectibleData data = JsonUtility.FromJson<CollectibleData>(json);

            Dictionary<Collectible.CollectibleType, int> collectibleCounts = new Dictionary<Collectible.CollectibleType, int>();
            foreach (var collectible in data.counts)
            {
                collectibleCounts[collectible.type] = collectible.count;
            }

            Debug.Log("Collectible counts loaded from " + filePath);
            return collectibleCounts;
        }
        else
        {
            CreateCollectibleCountsFile();
            return InitializeDefaultCollectibleCounts();
        }
    }

    private static Dictionary<Collectible.CollectibleType, int> InitializeDefaultCollectibleCounts()
    {
        Dictionary<Collectible.CollectibleType, int> defaultCounts = new Dictionary<Collectible.CollectibleType, int>();
        foreach (Collectible.CollectibleType type in System.Enum.GetValues(typeof(Collectible.CollectibleType)))
        {
            defaultCounts[type] = 0;
        }
        return defaultCounts;
    }

    private static void CreateCollectibleCountsFile()
    {
        CollectibleData data=new CollectibleData(InitializeDefaultCollectibleCounts());

        string filePath = Application.persistentDataPath + "/collectibleCounts.json";
        string json=JsonUtility.ToJson(data,true);

        File.WriteAllText(filePath, json);

        Debug.Log("Created Default file");
    }
}
