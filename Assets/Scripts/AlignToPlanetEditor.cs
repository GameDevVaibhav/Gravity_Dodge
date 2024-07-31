using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlignToPlanet))]
public class AlignToPlanetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AlignToPlanet script = (AlignToPlanet)target;
        if (GUILayout.Button("Align Objects"))
        {
            script.AlignObjectsToPlanet();
        }
    }
}
