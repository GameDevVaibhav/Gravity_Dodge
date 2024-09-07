using UnityEngine;

public class LockCover : MonoBehaviour
{
   
    public Material coverMaterial;  // The material with the shader applied
    string shaderPropertyName = "_DissolveOffest";  // The name of the Vector3 property in the shader

    private Vector3 currentPropertyValue;  // To store the current property value

    private void Awake()
    {
        coverMaterial = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        //coverMaterial=GetComponent<Material>();
        // Get the current value of the shader property (assuming the shader property is a Vector3)
        currentPropertyValue = coverMaterial.GetVector(shaderPropertyName);
        Debug.Log("Current" + currentPropertyValue);
    }

    // Method to update the shader property based on progress percentage
    public void UpdateCoverProgress(float progressPercentage)
    {
        // Map the percentage to the range of y values (-1.3 to 1.3)
        float mappedYValue = Mathf.Lerp(1.3f, -1.3f, progressPercentage / 100f);
        Debug.Log("Mapped Value" + mappedYValue);
        // Update the y component of the Vector3 property
        currentPropertyValue.y = mappedYValue;

        // Apply the updated Vector3 value to the material
        coverMaterial.SetVector(shaderPropertyName, new Vector3(0, mappedYValue, 0));
    }
}
