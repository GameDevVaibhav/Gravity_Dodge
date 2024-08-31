using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkyboxColors
{
    public Color colorLight;
    public Color colorDark;
}

public class ChangeSkybox : MonoBehaviour
{
    Skybox skybox;
    public Material[] skyboxMaterials;
    public List<SkyboxColors> skyboxColors;

    // Reference to the cloud object and its material
    public Renderer cloudRenderer;
    private Material cloudMaterial;

    private void Awake()
    {
        skybox = GetComponent<Skybox>();

        // Get the cloud material
        if (cloudRenderer != null)
        {
            cloudMaterial = cloudRenderer.material;
        }
    }

    public void UpdateSkybox(int index)
    {
        // Change the skybox material
        skybox.material = skyboxMaterials[index];

        // Change the cloud material's color properties
        if (cloudMaterial != null && skyboxColors != null && index < skyboxColors.Count)
        {
            
            cloudMaterial.SetColor("_Color_Valley", skyboxColors[index].colorLight);
            cloudMaterial.SetColor("_Color_Peak", skyboxColors[index].colorDark);

            
            
        }
    }
}
