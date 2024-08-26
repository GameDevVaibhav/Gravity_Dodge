using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class VehicleSelectionManager : MonoBehaviour
{
    public GameObject vehicleButtonPrefab; // Assign the VehicleButton prefab in the inspector
    public Transform vehicleButtonHolder; // Assign the Content object (or panel) where buttons will be instantiated
    public GameObject playerObject; // Assign the player object in the inspector
    public List<VehicleData> availableVehicles; // List of available vehicles as ScriptableObjects

    private GameObject currentVehicle;

    private void Start()
    {
        PopulateVehicleSelectionUI();
    }

    // Populates the vehicle selection UI with buttons
    private void PopulateVehicleSelectionUI()
    {
        foreach (var vehicle in availableVehicles)
        {
            GameObject button = Instantiate(vehicleButtonPrefab, vehicleButtonHolder);
            button.transform.GetComponent<Image>().sprite = vehicle.thumbnail;
            button.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = vehicle.vehicleName;

            // Add an onClick listener to equip the vehicle
            button.GetComponent<Button>().onClick.AddListener(() => EquipVehicle(vehicle));
        }
    }

    // Equips the selected vehicle to the player
    public void EquipVehicle(VehicleData vehicle)
    {
        if (currentVehicle != null)
        {
            Destroy(currentVehicle); // Destroy the current vehicle
        }

        // Instantiate the new vehicle as a child of the player object
        currentVehicle = Instantiate(vehicle.prefab, playerObject.transform);
        currentVehicle.transform.localPosition = Vector3.zero; // Adjust position if necessary
    }
}
