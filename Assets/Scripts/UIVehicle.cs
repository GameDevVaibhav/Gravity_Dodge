using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVehicle : MonoBehaviour
{
    public static UIVehicle Instance;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ActivateDisplayVechicle(GameObject vehicle)
    {
        if(gameObject.transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        Instantiate(vehicle, transform);
    }
}
