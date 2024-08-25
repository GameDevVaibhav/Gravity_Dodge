using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockCondition
{
    public Collectible.CollectibleType collectibleType;
    public int requiredAmount;
}

[System.Serializable]
public class PlanetUnlockConditions
{
    public List<UnlockCondition> unlockConditions;
}

public class PlanetUnlockCondition : MonoBehaviour
{
    [Tooltip("List of planet unlock conditions where each index corresponds to a planet.")]
    public PlanetUnlockConditions[] planetUnlockConditions;

    private bool[] isUnlocked;

    private void Awake()
    {
        // Initialize isUnlocked array based on the number of planets
        isUnlocked = new bool[planetUnlockConditions.Length];
    }
    private void Start()
    {
        

        // Load initial unlock statuses
        CheckAllUnlockStatuses();
    }

    public void CheckAllUnlockStatuses()
    {
        Dictionary<Collectible.CollectibleType, int> collectibleCounts = DataLoader.LoadCollectibleCounts();

        for (int i = 0; i < planetUnlockConditions.Length; i++)
        {
            if (!isUnlocked[i] && CheckUnlockStatusForPlanet(i, collectibleCounts))
            {
                UnlockPlanet(i);
            }
        }
    }

    private bool CheckUnlockStatusForPlanet(int planetIndex, Dictionary<Collectible.CollectibleType, int> collectibleCounts)
    {
        List<UnlockCondition> conditions = planetUnlockConditions[planetIndex].unlockConditions;

        foreach (var condition in conditions)
        {
            if (!collectibleCounts.ContainsKey(condition.collectibleType) ||
                collectibleCounts[condition.collectibleType] < condition.requiredAmount)
            {
                return false;
            }
        }

        return true;
    }

    private void UnlockPlanet(int planetIndex)
    {
        bool[] planetUnlockStatus = DataLoader.LoadPlanetUnlockedStatus(GameManager.Instance.PlanetCount);
        if (!planetUnlockStatus[planetIndex])
        {
            planetUnlockStatus[planetIndex] = true;
            DataLoader.SavePlanetUnlockedStatus(planetUnlockStatus);
            Debug.Log("Planet " + planetIndex + " has been unlocked!");
        }
        isUnlocked[planetIndex] = true;
    }
}

