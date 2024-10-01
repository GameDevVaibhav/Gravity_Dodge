using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehicleUnlockConditions
{
    public List<UnlockCondition> unlockConditions;
}

public class VehicleUnlockCondition : MonoBehaviour
{
    [Tooltip("List of vehicle unlock conditions where each index corresponds to a vehicle.")]
    public VehicleUnlockConditions[] vehicleUnlockConditions;

    private bool[] isUnlocked;

    private void Awake()
    {
        // Initialize isUnlocked array based on the number of vehicles
        isUnlocked = new bool[vehicleUnlockConditions.Length];
    }

    private void Start()
    {
        // Load initial unlock statuses
        CheckAllUnlockStatuses();
    }

    public void CheckAllUnlockStatuses()
    {
        Dictionary<Collectible.CollectibleType, int> collectibleCounts = DataLoader.LoadCollectibleCounts();

        for (int i = 0; i < vehicleUnlockConditions.Length; i++)
        {
            if (!isUnlocked[i] && CheckUnlockStatusForVehicle(i, collectibleCounts))
            {
                UnlockVehicle(i);
            }
        }
    }

    private bool CheckUnlockStatusForVehicle(int vehicleIndex, Dictionary<Collectible.CollectibleType, int> collectibleCounts)
    {
        List<UnlockCondition> conditions = vehicleUnlockConditions[vehicleIndex].unlockConditions;

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

    private void UnlockVehicle(int vehicleIndex)
    {
        bool[] vehicleUnlockStatus = DataLoader.LoadVehicleUnlockedStatus(vehicleUnlockConditions.Length);
        if (!vehicleUnlockStatus[vehicleIndex])
        {
            vehicleUnlockStatus[vehicleIndex] = true;
            DataLoader.SaveVehicleUnlockedStatus(vehicleUnlockStatus);
            Debug.Log("Vehicle " + vehicleIndex + " has been unlocked!");
            NotificationManager.Instance.ShowNotification("New Vehicle Unlocked");
        }
        isUnlocked[vehicleIndex] = true;
    }
}
