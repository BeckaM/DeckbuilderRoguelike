using UnityEngine;
using UnityEditor;
using Assets.Scripts.DAL;

[CustomEditor(typeof(PlayerClassEditor))]
public class InspectorClass : Editor
{    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerClassEditor myScript = (PlayerClassEditor)target;
        if (GUILayout.Button("Get all classes"))
        {
            myScript.GetClassesToEdit();
        }

        if (GUILayout.Button("Save"))
        {
            myScript.SaveClasses();
        }
    }
}

