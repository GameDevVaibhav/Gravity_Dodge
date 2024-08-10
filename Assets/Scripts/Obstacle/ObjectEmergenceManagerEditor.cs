using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectEmergenceManager))]
public class ObjectEmergenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectEmergenceManager manager = (ObjectEmergenceManager)target;

        if (GUILayout.Button("Assign Spawned Objects"))
        {
            AlignToPlanet alignToPlanet = FindObjectOfType<AlignToPlanet>();
            if (alignToPlanet != null)
            {
                manager.AssignSpawnedObjectsToOscillateArray(alignToPlanet.GetSpawnedObjects());
            }
            else
            {
                Debug.LogWarning("AlignToPlanet component not found in the scene.");
            }
        }
    }
}
