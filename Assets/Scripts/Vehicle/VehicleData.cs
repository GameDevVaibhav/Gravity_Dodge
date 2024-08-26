using UnityEngine;

[CreateAssetMenu(fileName = "NewVehicleData", menuName = "Vehicle Data", order = 51)]
public class VehicleData : ScriptableObject
{
    public string vehicleName;
    public Sprite thumbnail;
    public GameObject prefab;
}
