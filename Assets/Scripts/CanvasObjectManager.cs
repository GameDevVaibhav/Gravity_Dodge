using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasObjectManager : MonoBehaviour
{
    [SerializeField]
    Button vehicleSelect;
    [SerializeField]
    Button backButton;

    [SerializeField]
    GameObject vehicleSelectPanel;


    private void Awake()
    {
        vehicleSelect.onClick.AddListener(()=>TogglePanels(vehicleSelectPanel,true));
        backButton.onClick.AddListener(()=>TogglePanels(vehicleSelectPanel,false));
    }

    void TogglePanels(GameObject panel,bool active)
    {
        panel.SetActive(active);
    }
}
