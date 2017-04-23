using UnityEngine;
using UnityEditor;
using Assets.Scripts.DAL;

[CustomEditor(typeof(PerkEditor))]
public class InspectorPerk : Editor
{    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PerkEditor myScript = (PerkEditor)target;
        if (GUILayout.Button("Get all perks"))
        {
            UnityEditor.AssetDatabase.Refresh();
            myScript.GetPerksToEdit();
        }

        if (GUILayout.Button("Save"))
        {
            myScript.SavePerks();
        }
    }
}

