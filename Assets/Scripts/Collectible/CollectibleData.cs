using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectibleData
{
    public List<CollectibleCount> counts = new List<CollectibleCount>();

    public CollectibleData(Dictionary<Collectible.CollectibleType, int> collectibleCounts)
    {
        foreach (var pair in collectibleCounts)
        {
            counts.Add(new CollectibleCount(pair.Key, pair.Value));
        }
    }
}

[System.Serializable]
public class CollectibleCount
{
    public Collectible.CollectibleType type;
    public int count;

    public CollectibleCount(Collectible.CollectibleType type, int count)
    {
        this.type = type;
        this.count = count;
    }
}
