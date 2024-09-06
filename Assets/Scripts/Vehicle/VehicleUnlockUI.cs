using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUnlockUI : MonoBehaviour
{
    public GameObject conditionEntryPrefab; // The prefab for displaying a condition
    public Transform conditionsContainer; // The container for all condition entries

    public VehicleUnlockCondition vehicleUnlockCondition; // Reference to the VehicleUnlockCondition script
    public VehicleSelectionManager vehicleSelectionManager; // Reference to the VehicleSelectionManager script

    private void Start()
    {
        vehicleUnlockCondition = FindObjectOfType<VehicleUnlockCondition>();
        UpdateUnlockConditionsUI();
    }

    public void UpdateUnlockConditionsUI()
    {
        // Clear existing condition entries
        foreach (Transform child in conditionsContainer)
        {
            Destroy(child.gameObject);
        }

        // Get the unlock conditions for the selected vehicle
        int selectedVehicleIndex = vehicleSelectionManager.availableVehicles.IndexOf(vehicleSelectionManager.selectedVehicleData);
        if (selectedVehicleIndex < 0) return;

        VehicleUnlockConditions unlockConditions = vehicleUnlockCondition.vehicleUnlockConditions[selectedVehicleIndex];
        Dictionary<Collectible.CollectibleType, int> collectibleCounts = DataLoader.LoadCollectibleCounts();

        foreach (UnlockCondition condition in unlockConditions.unlockConditions)
        {
            // Instantiate a new condition entry
            GameObject conditionEntry = Instantiate(conditionEntryPrefab, conditionsContainer);


            // Set the initial scale to zero for "pop-in" effect
            conditionEntry.transform.localScale = Vector3.zero;

            // Animate the scale from 0 to 1 using DoTween to create a pop effect
            conditionEntry.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);  // Customize ease and duration as needed

            // Get the components from the instantiated prefab
            Image collectibleImage = conditionEntry.transform.GetChild(1).GetComponent<Image>();
            TextMeshProUGUI progressText = conditionEntry.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            // Set the collectible sprite
            Sprite collectibleSprite = CollectibleSpriteManager.Instance.GetSpriteForType(condition.collectibleType);
            collectibleImage.sprite = collectibleSprite;

            // Calculate the current amount of collected items and set the progress text
            int currentAmount = collectibleCounts.ContainsKey(condition.collectibleType) ? collectibleCounts[condition.collectibleType] : 0;
            progressText.text = $"{currentAmount}/{condition.requiredAmount}";

            // Optionally, you could disable the prefab if the condition is met
            // if (currentAmount >= condition.requiredAmount)
            // {
            //     progressText.color = Color.green; // Indicate completion
            // }
            // else
            // {
            //     progressText.color = Color.red; // Indicate remaining
            // }
        }
    }
}
