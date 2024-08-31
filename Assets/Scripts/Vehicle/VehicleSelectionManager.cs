using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VehicleSelectionManager : MonoBehaviour
{
    public GameObject vehicleButtonPrefab; // Assign the VehicleButton prefab in the inspector
    public Transform vehicleButtonHolder; // Assign the Content object (or panel) where buttons will be instantiated
    public GameObject playerObject; // Assign the player object in the inspector
    public Image selectedVehicleThumbnail; // UI Image to show the vehicle's thumbnail when selected
    public TextMeshProUGUI selectedVehicleName; // UI Text to show the vehicle's name when selected
    public Button equipButton; // Assign the Equip button in the inspector
    public List<VehicleData> availableVehicles; // List of available vehicles as ScriptableObjects

    public bool[] vehicleUnlockStatus; // Array to track which vehicles are unlocked (true = unlocked, false = locked)
    private GameObject currentVehicle; // The currently equipped vehicle
    public VehicleData selectedVehicleData; // The vehicle data of the currently selected vehicle
    public VehicleUnlockUI vehicleUnlockUI;


    private void Start()
    {
        LoadVehicleUnlockStatus();
        PopulateVehicleSelectionUI();
        equipButton.onClick.AddListener(EquipSelectedVehicle);
        equipButton.interactable = false; // Disable the equip button until a vehicle is selected
        selectedVehicleData = availableVehicles[0];
        SelectVehicle(selectedVehicleData);
        EquipSelectedVehicle();
    }

    // Load the vehicle unlock statuses from the JSON file using the DataLoader script
    public void LoadVehicleUnlockStatus()
    {
        int vehicleCount = availableVehicles.Count; // Get the number of vehicles
        vehicleUnlockStatus = DataLoader.LoadVehicleUnlockedStatus(vehicleCount);

        // Ensure the array is initialized with a default value if no data was loaded
        if (vehicleUnlockStatus == null || vehicleUnlockStatus.Length != vehicleCount)
        {
            vehicleUnlockStatus = new bool[vehicleCount];
            for (int i = 0; i < vehicleCount; i++)
            {
                vehicleUnlockStatus[i] = false; // Default all vehicles to locked if no data is loaded
            }
        }
    }

    // Populates the vehicle selection UI with buttons
    private void PopulateVehicleSelectionUI()
    {
        foreach (var vehicle in availableVehicles)
        {
            GameObject button = Instantiate(vehicleButtonPrefab, vehicleButtonHolder);
            button.transform.Find("Icon").GetComponent<Image>().sprite = vehicle.thumbnail;
            button.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = vehicle.vehicleName;

            // Add an onClick listener to select the vehicle (but not equip it yet)
            button.GetComponent<Button>().onClick.AddListener(() => SelectVehicle(vehicle));
        }
    }

    // Selects a vehicle to be shown in the UI
    public void SelectVehicle(VehicleData vehicle)
    {
        selectedVehicleData = vehicle;
        selectedVehicleThumbnail.sprite = vehicle.thumbnail;
        selectedVehicleName.text = vehicle.vehicleName;
        equipButton.interactable = true; // Enable the equip button now that a vehicle is selected
        vehicleUnlockUI.UpdateUnlockConditionsUI();
        UIVehicle.Instance.ActivateDisplayVechicle(selectedVehicleData.prefab);
}

    // Equips the selected vehicle
    private void EquipSelectedVehicle()
    {
        if (selectedVehicleData != null)
        {
            int vehicleIndex = availableVehicles.IndexOf(selectedVehicleData);

            if (vehicleIndex >= 0 && vehicleIndex < vehicleUnlockStatus.Length && vehicleUnlockStatus[vehicleIndex])
            {
                if (currentVehicle != null)
                {
                    Destroy(currentVehicle); // Destroy the current vehicle
                }

                // Instantiate the new vehicle as a child of the player object
                currentVehicle = Instantiate(selectedVehicleData.prefab, playerObject.transform);
                currentVehicle.transform.localPosition = Vector3.zero; // Adjust position if necessary

                // Optionally, disable the equip button after equipping
                equipButton.interactable = false;
            }
            else
            {
                Debug.Log("This vehicle is locked and cannot be equipped!");
            }
        }
    }
}
