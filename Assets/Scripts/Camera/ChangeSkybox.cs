using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    Skybox skybox;
    public Material[] skyboxPrefab;
    private void Awake()
    {
        skybox = GetComponent<Skybox>();
    }

    public void UpdateSkybox(int index)
    {
        skybox.material = skyboxPrefab[index];
    }
}
