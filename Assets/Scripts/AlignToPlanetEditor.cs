using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlignToPlanet))]
public class AlignToPlanetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AlignToPlanet script = (AlignToPlanet)target;

        // Button to align and position objects
        if (GUILayout.Button("Align and Position Objects"))
        {
            script.AlignAndPositionObjects();
        }
    }
}
