using UnityEngine;
using UnityEditor;
using Assets.Scripts.DAL;

[CustomEditor(typeof(FullCardEditor))]
public class CardEditorInspector : Editor
{    
    public string cardToEdit;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FullCardEditor myScript = (FullCardEditor)target;
        if (GUILayout.Button("Get All Cards"))
        {
            myScript.GetCardsToEdit();
        }

        if (GUILayout.Button("Clear"))
        {
            myScript.Clear();
        }

        if (GUILayout.Button("Edit This Card"))
        {
            myScript.EditCard();
        }

        if (GUILayout.Button("Apply Card Changes"))
        {
            myScript.ApplyCardChanges();
        }

        if (GUILayout.Button("Save"))
        {
            myScript.SaveCards();
        }
    }
}

