using UnityEngine;
using UnityEditor;
using Assets.Scripts.DAL;

[CustomEditor(typeof(ItemEditor))]
public class InspectorItems : Editor
{    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemEditor myScript = (ItemEditor)target;
        if (GUILayout.Button("Get all Items"))
        {
            UnityEditor.AssetDatabase.Refresh();
            myScript.GetItemsToEdit();
        }

        if (GUILayout.Button("Save"))
        {
            myScript.SaveItems();
        }
    }
}

