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
    public TextMeshProUGUI equipButtonText; // Reference to the equip button's text
    public List<VehicleData> availableVehicles; // List of available vehicles as ScriptableObjects

    public bool[] vehicleUnlockStatus; // Array to track which vehicles are unlocked (true = unlocked, false = locked)
    private GameObject currentVehicle; // The currently equipped vehicle
    public VehicleData selectedVehicleData; // The vehicle data of the currently selected vehicle
    private VehicleData currentEquippedVehicle;
    public VehicleUnlockUI vehicleUnlockUI;


    private void Start()
    {
        LoadVehicleUnlockStatus();
        PopulateVehicleSelectionUI();
        equipButton.onClick.AddListener(EquipSelectedVehicle);
       
        selectedVehicleData = availableVehicles[0];
        currentEquippedVehicle = selectedVehicleData;
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

    public void SelectVehicle(VehicleData vehicle)
    {
        selectedVehicleData = vehicle;
        selectedVehicleThumbnail.sprite = vehicle.thumbnail;
        selectedVehicleName.text = vehicle.vehicleName;

        // Check vehicle lock status and update equip button text
        int vehicleIndex = availableVehicles.IndexOf(vehicle);
        if (vehicleUnlockStatus[vehicleIndex])
        {
            if (currentEquippedVehicle == selectedVehicleData) // Vehicle is already equipped
            {
                equipButtonText.text = "Equipped";
                SetButtonColor("#7B7974");
            }
            else
            {
                equipButtonText.text = "Equip";
                SetButtonColor("#FFD407");
                
            }
        }
        else
        {
            equipButtonText.text = "Locked";
            SetButtonColor("red");
        }

        //equipButton.interactable = vehicleUnlockStatus[vehicleIndex];  // Enable button if unlocked
        vehicleUnlockUI.UpdateUnlockConditionsUI();  // Update the unlock conditions UI
        UIVehicle.Instance.ActivateDisplayVechicle(selectedVehicleData.prefab);  // Show vehicle in display
    }

    private void SetButtonColor(string colorCode)
    {
        // Parse the hex color code and set the button color
        Color color;
        if (ColorUtility.TryParseHtmlString(colorCode, out color))
        {
            Image buttonImage = equipButton.GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.color = color;  // Set the button's background color
            }
        }
    }

    // Equips the selected vehicle
    public void EquipSelectedVehicle()
    {
        if (selectedVehicleData != null)
        {
            int vehicleIndex = availableVehicles.IndexOf(selectedVehicleData);

            // Check if the vehicle is unlocked
            if (vehicleIndex >= 0 && vehicleIndex < vehicleUnlockStatus.Length && vehicleUnlockStatus[vehicleIndex])
            {
                // Destroy the currently equipped vehicle if any
                if (currentVehicle != null)
                {
                    Destroy(currentVehicle);
                }

                // Instantiate the new vehicle as a child of the player object
                currentVehicle = Instantiate(selectedVehicleData.prefab, playerObject.transform);
                currentVehicle.transform.localPosition = Vector3.zero;  // Adjust position if necessary

                // Update the currently equipped vehicle
                currentEquippedVehicle = selectedVehicleData;

                // Update the equip button text to show that the vehicle is equipped
                equipButtonText.text = "Equipped";

                
            }
            else
            {
                Debug.Log("This vehicle is locked and cannot be equipped!");
            }
        }
    }
}
